using Godot;
using System.Linq;

public partial class Spawner : Node2D
{
    //Esiladataan putoava peliobjekti
	PackedScene FallingObject = GD.Load<PackedScene>("res://scenes/FallingObject.tscn");

    private readonly RandomNumberGenerator _rng = new();

    private ReferenceRect SpawnArea;
    private Timer _spawnTimer;
    private int _scoreThresholdForAcceleration = 5;
    private const float _spawningAcceleration = 0.85f;
    private const float _spawningAccelerationCap = 0.35f;

    public override void _Ready()
    {
        SpawnArea = GetChildren().OfType<ReferenceRect>().Single(); //Haetaan, child-nodea tyypiltä ReferenceRect
        _rng.Randomize();

        _spawnTimer = new Timer //Luodaan uusi Timer-objekti ja annetaan sille pari oletusasetusta
        {
            WaitTime = 4f, //Ajastin lähtee neljästä alaspäin
            Autostart = true //Ajastin käynnistyy automaattisesti
        };
        AddChild(_spawnTimer); //Objektin luomisen jälkeen se pitää vielä lisätä kohtaukseen.
        _spawnTimer.Timeout += SpawnObject; //Ruvetaan kuuntelemaan, milloin ajastin on nollassa, jolloin kutsutaan SpawnObject-metodia.
    }

    public override void _Process(double delta)
    {
        if (GameManager.Instance.Score >= _scoreThresholdForAcceleration) //Tarkistetaan, ylittävätkö pisteet rajan, jonka ylittämisestä putoavien objektien luominen nopeutuu.
        {
            _scoreThresholdForAcceleration = GameManager.Instance.Score + 5; //Jos raja ylitettiin asetetaan uusi raja, joka on pisteiden määrä + 5
            _spawnTimer.WaitTime = Mathf.Max(_spawnTimer.WaitTime * _spawningAcceleration, _spawningAccelerationCap); //Kiihdytetään uusien objektien luomisnopeutta laskemalla ajastimen lähtöarvoa.

            GD.Print($"Speeding up! \nNew objects are spawned now every {_spawnTimer.WaitTime} seconds.");
            GD.Print($"Game will get harder when player reaches score of {_scoreThresholdForAcceleration}.");
        }
    }

    private void SpawnObject()
    {
        Rect2 area = SpawnArea.GetGlobalRect();
        
        var obj = FallingObject.Instantiate<FallingObject>();
        obj.GlobalPosition = new Vector2(_rng.RandfRange(area.Position.X, area.End.X), area.Position.Y);
        AddChild(obj);
        
        SoundManager.Instance.PlaySfx(Sfx.Kick, true);
    }
}
