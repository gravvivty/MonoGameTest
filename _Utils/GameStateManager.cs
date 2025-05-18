using System;

public enum GameState
{
    MainMenu,
    Playing,
    Options,
    Paused,
}

public static class GameStateManager
{
    public static GameState CurrentGameState { get; private set; } = GameState.MainMenu;

    public static void ChangeGameState(GameState newGameState)
    {
        CurrentGameState = newGameState;
        Console.WriteLine($"Switching from {CurrentGameState} to: {newGameState}");
    }
}
