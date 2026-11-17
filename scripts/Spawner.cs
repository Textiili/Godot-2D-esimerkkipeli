using Godot;
using System;
using System.Linq;

public partial class Spawner : Node2D
{
	PackedScene FallingObject = GD.Load<PackedScene>("res://scenes/FallingObject.tscn");
    private readonly RandomNumberGenerator _rng = new();
    private ReferenceRect SpawnArea;
    private Timer _spawnTimer;
    private int _scoreThresholdForAcceleration = 5;
    private const float _acceleration = 0.85f;
    private const float _accelerationCap = 0.35f;

    public override void _Ready()
    {
        SpawnArea = GetChildren().OfType<ReferenceRect>().Single();
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
            _spawnTimer.WaitTime = Mathf.Max(_spawnTimer.WaitTime * _acceleration, _accelerationCap);

            GD.Print($"Speeding up! \nNew objects are spawned now every {_spawnTimer.WaitTime} seconds.");
            GD.Print($"Game will get harder when player reaches score of {_scoreThresholdForAcceleration}.");
        }
    }

    private void SpawnObject()
    {
        Rect2 area = SpawnArea.GetGlobalRect();

        var obj = FallingObject.Instantiate<FallingObject>();
        obj.GlobalPosition = new Vector2(_rng.RandfRange(area.Position.X, area.End.X), area.Position.Y);

        obj.Caught += OnCaught;
        obj.Missed += OnMissed;
        AddChild(obj);

        SoundManager.Instance.PlaySfx(Sfx.Kick, true);
    }

    private void OnCaught() => GameManager.Instance.AddScore();

    private void OnMissed() => GameManager.Instance.GameOver();
}
