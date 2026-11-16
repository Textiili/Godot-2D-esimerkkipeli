using Godot;

public partial class Spawner : Node2D
{
	PackedScene FallingObject = GD.Load<PackedScene>("res://scenes/FallingObject.tscn");

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
        _spawnTimer.WaitTime = 4f;
        _spawnTimer.Start();
        GD.Print("Spawner startattu!");
    }

    public override void _Process(double delta)
    {
        if (GameManager.Instance.Score >= _threshold)
        {
            _threshold = GameManager.Instance.Score + 5;
            _spawnTimer.WaitTime = _spawnTimer.WaitTime * 0.8;
            GD.Print(_threshold); //TODO: Debug?
            GD.Print(_spawnTimer.WaitTime);
        }
    }

    private void SpawnObject()
    {
        //TODO: Sound?
        var obj = FallingObject.Instantiate<FallingObject>();
        float width = GetViewportRect().Size.X;
        obj.Position = new Vector2(_rng.RandfRange(32, width - 32), -32);

        obj.Caught += OnCaught;
        obj.Missed += OnMissed;
        AddChild(obj);
    }

    private void OnCaught() => GameManager.Instance.AddScore();

    private void OnMissed()
    {
        GameManager.Instance.GameOver();
    }
}
