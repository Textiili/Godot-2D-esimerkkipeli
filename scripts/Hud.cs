using Godot;

public partial class Hud : CanvasLayer
{
	private Label _highScoreLabel;
	private Label _scoreLabel;
	private Label _gameOverLabel;
	public override void _Ready()
	{
		//Sallitaan, että UI voi päivittyä, kun peli on pysäytetty.
		ProcessMode = ProcessModeEnum.Always;

		//Haetaan päivitettävät tekstielementit Hudin solmurakenteesta
		_highScoreLabel = GetNode<Label>("HudRoot/HighScore");
		_scoreLabel = GetNode<Label>("HudRoot/Score");
		_gameOverLabel = GetNode<Label>("HudRoot/GameOver");

		//Asetetaan highscore tekstielementtiin, kun kohtaus ladataan uudelleen.
		UpdateHighScoreLabel(GameManager.Instance.HighScore);
		GameManager.Instance.HighScoreChanged += UpdateHighScoreLabel;

		//Asetetaan score tekstielementtiin ja ruvetaan kuuntelemaan signaaleja.
		UpdateScoreLabel(GameManager.Instance.Score); //Haetaan lähtöpisteet.
		GameManager.Instance.ScoreChanged += UpdateScoreLabel; //Kuunnellaan signaaleja uusista pisteistä.
	}

  public override void _ExitTree()
	{
		if (GameManager.Instance != null) 
			GameManager.Instance.ScoreChanged -= UpdateScoreLabel;
			GameManager.Instance.HighScoreChanged -= UpdateHighScoreLabel;
	}

	private void UpdateHighScoreLabel(int highScore)
	{
		_highScoreLabel.Text = $"Highscore:\n{highScore}";
	}

	private void UpdateScoreLabel(int score)
	{
		_scoreLabel.Text = $"Score:\n{score}";
	}
}
