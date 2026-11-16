using Godot;
using System;

public enum GameState //Remove?
{
	MainMenu,
	Starting, 
	Active,
	GameOver
}

public partial class GameManagerTest : Node
{
	public static GameManagerTest Instance {get; private set; }
	public int Score {get; private set; }
	public event Action<int> ScoreChanged;
	public int HighScore { get; private set; }

	private const string LevelScenePath = "res://scenes/Level.tscn";

	public GameState CurrentState { get; private set; } = GameState.MainMenu;

	public event Action<GameState, GameState> StateChanged;

	public override void _Ready()
	{
		Instance = this;
		ProcessMode = ProcessModeEnum.Always;
	}

  public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("quit_game"))
		{
			GetTree().Quit();
		}
		if (Input.IsActionJustPressed("restart")) //TODO: Remove?
		{
			NewGame();
		}
	}

	public async void NewGame()
	{
		//Muutetaan uutta peliä varten pisteet nollaan ja ilmoitetaan HUD:lle muutoksesta
		Score = 0;
		ScoreChanged?.Invoke(Score);

		//Laitetaan paussi pois päältä
		GetTree().Paused = false;

		//Ladataan pelikenttä
		GetTree().ChangeSceneToFile(LevelScenePath);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		ChangeState(GameState.Starting);
	}

	public void AddScore()
	{
		//IF STATE?
		Score += 1;
		ScoreChanged?.Invoke(Score); //Viestii UI:lle pelin aikana pisteiden muutokset.
	}

	public void UpdateHighScore(int score)
	{
		if (score > HighScore)
		{
			HighScore = score;
		}
	}

	public void GameOver()
	{
		GetTree().Paused = true;
		UpdateHighScore(Score);

		// if (CurrentState != GameState.Active) return;
		// ChangeState(GameState.GameOver);
	}

	private void ChangeState(GameState newState) //REMOVE?
	{
		if (newState == CurrentState) return;

		GameState oldState = CurrentState;
		
		GD.Print($"GameState: {oldState} -> {newState}");
	}
}
