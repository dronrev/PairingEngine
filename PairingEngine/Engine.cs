namespace PairingEngine;

public class Engine
{
    public RoundTournament GenerateRound(Tournament tournament)
    {
        var roundDetails = new RoundTournament
        {
            RoundNumber = 1
        };
        var sortedPlayers = tournament.Players.OrderByDescending(player => player.Rating).ToList();
        var tableCount = sortedPlayers.Count/2;
        var currentTable = 1;
        for (var i = 0; i < tableCount; i++)
        {
            roundDetails.GameDetailsList.Add(new GameDetails
            {
                TableNumber = currentTable++,
                WhitePlayerId = sortedPlayers[i].PlayerId,
                BlackPlayerId = sortedPlayers[tableCount + i].PlayerId,
            });
        }
        
        return roundDetails;
    }
}