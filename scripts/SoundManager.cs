using Godot;
using System.Collections.Generic;

public enum Sfx { FullTime, Kick, ScoringPoint }

public partial class SoundManager : Node
{
	public static SoundManager Instance {get; private set; }

	private const int PoolSize = 8;
	private const double MinRepeatDelay = 0.05;

	private readonly Dictionary<Sfx, AudioStream> _streams = new();
	private readonly Dictionary<Sfx, double> _lastPlayed = new();
	private readonly List<AudioStreamPlayer> _pool = new();

	private readonly RandomNumberGenerator _rng = new();

	public override void _Ready()
	{
		Instance = this;
		_rng.Randomize();
		ProcessMode = ProcessModeEnum.Always;

		//Ladataan ääniefektit muistiin ja asetetaan ne _streams-sanakirjaan
		_streams[Sfx.FullTime] = GD.Load<AudioStream>("res://assets/audio/sfx_full_time.wav");
		_streams[Sfx.Kick] = GD.Load<AudioStream>("res://assets/audio/sfx_kick.wav");
		_streams[Sfx.ScoringPoint] = GD.Load<AudioStream>("res://assets/audio/sfx_scoring_point.wav");

		//Luodaan PoolSize verran AudioStreamPlayer-nodeja
		for (int i = 0; i < PoolSize; i++)
		{
			var audioPlayer = new AudioStreamPlayer { Bus = "SFX" };
			AddChild(audioPlayer);
			_pool.Add(audioPlayer); //Lisätään ne AudioStreamPlayer-listaan nimeltään _pool
		}
	}

	public void PlaySfx(Sfx sfx, bool randomizePitch = false) //Vaaditaan parametriksi enum Sfx:lle ja voidaan syöttää true jos halutaan antaa satunnainen korkeus
	{
		double now = Time.GetTicksMsec() / 1000.0;

		if (_lastPlayed.TryGetValue(sfx, out double last) && now - last < MinRepeatDelay)
			return; //Jos x niin ei suoriteta metodia loppuun.

		AudioStreamPlayer free = _pool.Find(p => !p.Playing); 
		if (free == null) //Jos ei löydetä vapaata audiolähdettä metodia ei suoriteta loppuun.
			return;

		free.Stream = _streams[sfx]; //Jos löytyi vapaa audiolähde, asetetaan parametriksi annettu sfx
		free.PitchScale = randomizePitch ? _rng.RandfRange(0.95f, 1.05f) : 1f; //Jos oli annettu parametri randomizePitch true, annetaan satunnainen korkeus
		free.Play(); //Toistetaan sfx

		_lastPlayed[sfx] = now; 
  }
}
