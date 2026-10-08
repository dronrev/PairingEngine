using System.Collections.Generic;
using System.Linq;

namespace PairingEngine;

public static class TieBreakerHelper
{
    private static List<decimal> CountOpponentScores(TournamentPlayer tournamentPlayer, Dictionary<string, TournamentPlayer> players)
    {
        return tournamentPlayer.Rounds
            .Where(r => r.IsPlayed && r.OpponentId is not null)
            .Select(r => players[r.OpponentId!].Points)
            .ToList();

    }
    
    public static decimal CountBuch(TournamentPlayer tournamentPlayer, Dictionary<string, TournamentPlayer> players)
    {
        return CountOpponentScores(tournamentPlayer, players).Sum();
    }
    
    public static decimal CountBuchCut1(TournamentPlayer tournamentPlayer, Dictionary<string, TournamentPlayer> players)
    {
        var opponentsScore = CountOpponentScores(tournamentPlayer, players);
        return opponentsScore.OrderBy(r => r).Skip(1).Take(opponentsScore.Count - 2).Sum();
    }

    public static decimal CountSonnebornBerger(TournamentPlayer tournamentPlayer,
        Dictionary<string, TournamentPlayer> players)
    {
        var totalScores =  tournamentPlayer.Rounds
            .Where(r => r.IsPlayed && r.OpponentId is not null)
            .Select(r => players[r.OpponentId!].Points * ResultHelper.GetPointsEarned(r.Color, r.Result))
            .ToList();
        return totalScores.Sum();
    }
}