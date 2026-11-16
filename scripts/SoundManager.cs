using Godot;
using System.Collections.Generic;

public enum Sfx
{
	FullTime,
	Kick
}
public partial class SoundManager : Node
{
	public static SoundManager Instance {get; private set; }
	private readonly Dictionary<Sfx, AudioStream> _sfx = new();
	private AudioStreamPlayer _audioPlayer;
	public override void _Ready()
	{
		Instance = this;
		ProcessMode = ProcessModeEnum.Always;

		//Ladataan ääniefektit muistiin asettamalla ne TODO
		_sfx[Sfx.FullTime] = GD.Load<AudioStream>(
			"res://assets/audio/sfx_full_time.wav"
		);

		_sfx[Sfx.Kick] = GD.Load<AudioStream>(
			"res://assets/audio/sfx_kick.wav"
		);

		_audioPlayer = new AudioStreamPlayer
		{
			MaxPolyphony = 20,
			ProcessMode = ProcessModeEnum.Always
		};
		AddChild(_audioPlayer);
	}

	public void PlaySfx(Sfx sfxName)
	{
		if (!_sfx.TryGetValue(sfxName, out AudioStream sfx))
		{
				GD.PushWarning($"Sound not found: {sfx}");
				return;
		}
		_audioPlayer.Stream = sfx;

		if (sfxName == Sfx.Kick)
		{
			_audioPlayer.PitchScale = (float)GD.RandRange(0.8, 1.1);
		} 
		else
		{
			_audioPlayer.PitchScale = 1.0f;
		}

		_audioPlayer.Play();
	}
}
