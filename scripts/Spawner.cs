using Godot;

public partial class Spawner : Node2D
{
	PackedScene FallingObject = GD.Load<PackedScene>("res://scenes/FallingObject.tscn");
    private readonly RandomNumberGenerator _rng = new();
    private Timer _spawnTimer;
    private int _scoreThresholdForAcceleration = 5;
    private float _acceleration = 0.9f;

    public override void _Ready()
    {
        _rng.Randomize();

        _spawnTimer = new Timer
        {
            WaitTime = 4f,
            Autostart = true
        };
        AddChild(_spawnTimer);
        _spawnTimer.Timeout += SpawnObject;
    }

    public override void _Process(double delta)
    {
        if (GameManager.Instance.Score >= _scoreThresholdForAcceleration)
        {
            _scoreThresholdForAcceleration = GameManager.Instance.Score + 5;
            _spawnTimer.WaitTime = _spawnTimer.WaitTime * _acceleration;

            GD.Print($"Speeding up! \nNew objects are spawned now every {_spawnTimer.WaitTime} seconds.");
            GD.Print($"Game will get harder when player reaches score of {_scoreThresholdForAcceleration}.");
        }
    }

    private void SpawnObject()
    {
        var obj = FallingObject.Instantiate<FallingObject>();
        float width = GetViewportRect().Size.X;
        obj.Position = new Vector2(_rng.RandfRange(32, width - 32), -32); //TODO: SpawnPoint

        obj.Caught += OnCaught;
        obj.Missed += OnMissed;
        AddChild(obj);

        SoundManager.Instance.PlaySfx(Sfx.Kick);
    }

    private void OnCaught() => GameManager.Instance.AddScore();

    private void OnMissed() => GameManager.Instance.GameOver();
}
