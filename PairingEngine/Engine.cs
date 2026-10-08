using System.Linq;

namespace PairingEngine;

public class Engine
{
    public RoundTournament GenerateFirstRound(Tournament tournament)
    {
        var roundDetails = new RoundTournament
        {
            RoundNumber = 1
        };
        var sortedPlayers = tournament.Players
            .OrderByDescending(player => player.Rating).ThenBy(p => p.PlayerName)
            .ToList();
        TournamentPlayer? byePlayer = null;
        if (sortedPlayers.Count % 2 == 1)
        {
            byePlayer = sortedPlayers[^1];
            sortedPlayers.RemoveAt(sortedPlayers.Count - 1);
        }
        var tableCount = sortedPlayers.Count/2;
        var currentTable = 1;
        for (var i = 0; i < tableCount; i++)
        {
            var topPlayer = sortedPlayers[i];
            var bottomPlayer = sortedPlayers[i + 1];
            var topIsWhite = i % 2 == 0;
            
            roundDetails.GameDetailsList.Add(new GameDetails
            {
                TableNumber = currentTable++,
                WhitePlayerId = topIsWhite ? topPlayer.PlayerId : bottomPlayer.PlayerId,
                BlackPlayerId = topIsWhite  ? bottomPlayer.PlayerId : topPlayer.PlayerId,
            });
        }

        if (byePlayer is not null)
        {
            roundDetails.GameDetailsList.Add(new GameDetails
            {
                TableNumber = tableCount + 1,
                WhitePlayerId = byePlayer.PlayerId,
                BlackPlayerId = null, 
            });
        }
        
        return roundDetails;
    }

    public RoundTournament GenerateRound(Tournament tournament, int roundNumber)
    {
        var roundDetails = new RoundTournament
        {
            RoundNumber = roundNumber
        };
        
        var sortedPlayers = tournament.Players
            .OrderByDescending(player => player.Rating).ThenBy(p => p.PlayerName)
            .ToList();

        if (tournament.TournamentRanking is null)
        {
            return new RoundTournament{ RoundNumber = roundNumber };
        }

        var players = tournament.TournamentRanking.StandingRows;
        
        StandingRow? byePlayer = null;
        if (players.Count % 2 == 1)
        {
            byePlayer = players[^1];
            players.RemoveAt(players.Count - 1);
        }
        var tableCount = players.Count/2;
        var currentTable = 1;
        for (var i = 0; i < tableCount; i++)
        {
            var topPlayer = players[i];
            var bottomPlayer = players[i + 1];
            var topIsWhite = i % 2 == 0;
            var topPlayerProfile = sortedPlayers?.Where(x => x.PlayerId == topPlayer.PlayerId).FirstOrDefault();
            
            if (topPlayerProfile is null)
                return new RoundTournament{ RoundNumber = roundNumber };
            
            if(!HardRuleCheckingPass(topPlayerProfile, bottomPlayer.PlayerId))
                return new RoundTournament{ RoundNumber = roundNumber };
            
            roundDetails.GameDetailsList.Add(new GameDetails
            {
                TableNumber = currentTable++,
                WhitePlayerId = topIsWhite ? topPlayer.PlayerId : bottomPlayer.PlayerId,
                BlackPlayerId = topIsWhite  ? bottomPlayer.PlayerId : topPlayer.PlayerId,
            });
        }

        if (byePlayer is not null)
        {
            roundDetails.GameDetailsList.Add(new GameDetails
            {
                TableNumber = tableCount + 1,
                WhitePlayerId = byePlayer.PlayerId,
                BlackPlayerId = null, 
            });
        }
        
        return roundDetails;
    }

    private bool HardRuleCheckingPass(TournamentPlayer tournamentPlayer, string? opponentId = null)
    {
        if (tournamentPlayer.Opponents.Contains("BYE") && opponentId is null)
            return false;
        
        if (opponentId is not null && tournamentPlayer.Opponents.Contains(opponentId))
            return false;
        
        var currentColor = tournamentPlayer.Rounds.First().Color;
        var colorStreak = 0;
        for (int i = 1; i < tournamentPlayer.Rounds.Count(); i++)
        {
            if (tournamentPlayer.Rounds[i].OpponentId is null)
                continue;
            
            if(currentColor == tournamentPlayer.Rounds[i].Color)
                colorStreak++;
            else
            {
                colorStreak = 0;
            }
            currentColor = tournamentPlayer.Rounds[i].Color;
            if (colorStreak >= 3)
                return false;
        }

        var playerAsWhiteCount = tournamentPlayer.Rounds.Count(x => x.Color == EnumColor.White);
        var playerAsBlackCount = tournamentPlayer.Rounds.Count(x => x.Color == EnumColor.Black);
        
        if(Math.Abs(playerAsWhiteCount - playerAsBlackCount) > 2)
            return false;
        
        return true;
    }

    private bool SoftRuleCheckingPass(TournamentPlayer tournamentPlayer, string? opponentId = null)
    {
        //Soft rules (minimize)
        // Give each player their due color: the one they've had less often, or the opposite of their last game.
        // When both players want the same color, the higher-ranked player gets their preference.
        // Keep downfloats and upfloats to a minimum.
        
        return true;
    }
}