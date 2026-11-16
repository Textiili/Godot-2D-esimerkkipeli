using Godot;
using System;


public partial class GameManager : Node
{
	public static GameManager Instance {get; private set; }
	public int Score {get; private set; }
	public event Action<int> ScoreChanged;
	public int HighScore { get; private set; }

	private const string LevelScenePath = "res://scenes/Level.tscn";

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
		if (Input.IsActionJustPressed("restart")) //TODO: Remove, restart is only dev tool?
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
	}

	public void AddScore()
	{
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
	}
}
