namespace PairingEngine;

public class Engine
{
    public RoundTournament GenerateRound(Tournament tournament)
    {
        var sortedPlayers = tournament.Players.OrderByDescending(player => player.Rating);
        
        return new RoundTournament
        {
            RoundNumber = 1
        };
    }
}