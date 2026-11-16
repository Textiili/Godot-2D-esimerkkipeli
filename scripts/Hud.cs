using Godot;

public partial class Hud : CanvasLayer
{
	private Label _highScoreLabel;
	private Label _scoreLabel;
	private Label _gameOverLabel;
	public override void _Ready()
	{
		//Haetaan päivitettävät tekstielementit
		_highScoreLabel = GetNode<Label>("HighScore");
		_scoreLabel = GetNode<Label>("Score");
		_gameOverLabel = GetNode<Label>("GameOver");

		//Asetetaan highscore tekstielementtiin, kun kohtaus ladataan uudelleen.
		UpdateHighScoreLabel(GameManagerTest.Instance.HighScore);

		//Asetetaan score tekstielementtiin ja ruvetaan kuuntelemaan signaaleja.
		UpdateScoreLabel(GameManagerTest.Instance.Score); //Haetaan lähtöpisteet.
		GameManagerTest.Instance.ScoreChanged += UpdateScoreLabel; //Kuunnellaan signaaleja uusista pisteistä.
	}

  public override void _ExitTree()
	{
		if (GameManagerTest.Instance != null) 
			GameManagerTest.Instance.ScoreChanged -= UpdateScoreLabel;
	}

	private void UpdateHighScoreLabel(int highScore)
	{
		_highScoreLabel.Text = $"Highscore: {highScore}";
	}

	private void UpdateScoreLabel(int score)
	{
		_scoreLabel.Text = $"Score: {score}";
	}
}
