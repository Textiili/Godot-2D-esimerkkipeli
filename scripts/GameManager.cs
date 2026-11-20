using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance {get; private set; }
	public int Score {get; private set; }
	public int HighScore { get; private set; }

	public event Action<int> ScoreChanged;
	public event Action<int> HighScoreChanged;
	public event Action GameEnded;

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

	private void NewGame()
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
		SoundManager.Instance.PlaySfx(Sfx.ScoringPoint);
	}

	public void UpdateHighScore(int newScore)
	{
		if (newScore > HighScore)
		{
			HighScore = newScore;
			HighScoreChanged?.Invoke(HighScore);
		}
	}

	public void GameOver()
	{
		GameEnded?.Invoke();
		SoundManager.Instance.PlaySfx(Sfx.FullTime);
		GetTree().Paused = true;
		UpdateHighScore(Score);
	}
}
