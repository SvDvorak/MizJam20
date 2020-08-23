using System;

public static class GameState
{
	public static Dialogue Dialogue;

	public static bool Ingame;
	public static bool GameOver;

	public static Action GameStarted;

	public static void StartGame()
	{
		Ingame = true;
		GameStarted?.Invoke();
	}
}