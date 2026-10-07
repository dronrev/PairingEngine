namespace PairingEngine;

public static class ResultHelper
{
    public static decimal GetPointsEarned(EnumColor color, GameResult result) =>
        (color, result) switch
        {
            (_, GameResult.Draw)                    => 0.5m,
            (EnumColor.White, GameResult.WhiteWin)  => 1m,
            (EnumColor.Black, GameResult.BlackWin)  => 1m,
            _                                       => 0m 
        };
    
    public static bool IsWin(EnumColor color, GameResult result) =>
        (color, result) is (EnumColor.White, GameResult.WhiteWin)
        or (EnumColor.Black, GameResult.BlackWin);
}