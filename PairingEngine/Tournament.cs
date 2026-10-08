using System.Collections.Generic;

namespace PairingEngine;

public enum GameResult { Pending, WhiteWin, BlackWin, Draw }
public enum EnumCategory { Men, Women }

public enum TournamentFormat { Swiss, RoundRobin }

public class Tournament
{
    public required string TournamentId  { get; set; }
    public required string TournamentName { get; set; }
    public required int TotalRounds { get; set; }
    public TournamentRanking? TournamentRanking { get; set; }
    public required EnumCategory TournamentCategory { get; set; }
    public List<TournamentPlayer> Players { get; set; } = [];
    public TournamentFormat Format { get; set; }
}

public class TournamentRanking
{
    public List<StandingRow> StandingRows { get; set; } = [];
}

public class TournamentPlayer
{
    public required string PlayerId { get; init; }
    public required string PlayerName { get; set; }
    public int Rating { get; init; }
    public int StartingRank { get; set; }
    public List<string> Opponents { get; init; } = [];
    public decimal Points { get; set; }
    public List<RoundRecord> Rounds { get; } = new();
}


public class RoundRecord
{
    public required int Round { get; init; }
    public string? OpponentId { get; init; }
    public required EnumColor Color { get; set; }
    public required GameResult Result { get; set; }
    public bool IsPlayed { get; init; } = true;
    public bool IsWin { get; set; }

    public decimal PointEarned { get; set; }
}

public class StandingRow
{
    public required string PlayerId { get; init; }
    public required string PlayerName { get; init; }
    public int Rank { get; set; }
    public decimal Points { get; init; }
    public decimal BuchCut1 { get; init; }
    public decimal Buch { get; init; }
    public decimal SonnebornBerger { get; init; }
    public int Wins { get; init; }
}

public class RoundTournament
{
    public required int RoundNumber { get; init; }
    public List<GameDetails> GameDetailsList { get; set; } = [];
}

public class GameDetails
{
    public int TableNumber { get; set; }
    public string WhitePlayerId { get; set; } =  string.Empty;
    public string? BlackPlayerId { get; set; }
    public GameResult Result { get; set; } = GameResult.Pending;

    public bool IsBye => BlackPlayerId is null;
}