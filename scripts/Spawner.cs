using Godot;

public partial class Spawner : Node2D
{
	PackedScene FallingObject = GD.Load<PackedScene>("res://scenes/FallingObjectTest.tscn");

    private readonly RandomNumberGenerator _rng = new();
    private Timer _spawnTimer;
    private int _threshold = 5;
    private int _missed;
    private bool _gameOver;

    public override void _Ready()
    {
        _rng.Randomize();
        _spawnTimer = GetNode<Timer>("SpawnTimer");
        _spawnTimer.Timeout += SpawnObject;
        _spawnTimer.WaitTime = 4f; //Remove?
        _spawnTimer.Start();
        GD.Print("Spawner startattu!");
    }

    public override void _Process(double delta)
    {
        if (GameManagerTest.Instance.Score >= _threshold)
        {
            _threshold = GameManagerTest.Instance.Score + 5;
            _spawnTimer.WaitTime = _spawnTimer.WaitTime * 0.8;
            GD.Print(_threshold);
            GD.Print(_spawnTimer.WaitTime);
        }
    }

    private void SpawnObject()
    {
        var obj = FallingObject.Instantiate<FallingObject>();
        float width = GetViewportRect().Size.X;
        obj.Position = new Vector2(_rng.RandfRange(32, width - 32), -32);

        obj.Caught += OnCaught;
        obj.Missed += OnMissed;
        AddChild(obj);
    }

    private void OnCaught() => GameManagerTest.Instance.AddScore();

    private void OnMissed()
    {
        GameManagerTest.Instance.GameOver();
    }
}
