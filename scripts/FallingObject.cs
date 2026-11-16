using Godot;

public partial class FallingObject : Area2D
{
	[Signal] public delegate void CaughtEventHandler();
	[Signal] public delegate void MissedEventHandler();

	private float FallSpeed = 30f;

	public override void _Ready()
	{
	  BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
		Position += new Vector2(0, FallSpeed * (float)delta);
	}

	private void OnBodyEntered(Node2D body)
	{ 
		if (body.Name == "Player")
		{
			//TODO: Sound?
			EmitSignal(SignalName.Caught);
			QueueFree();
		} else if (body.Name == "Floor")
		{
			EmitSignal(SignalName.Missed);
			QueueFree();
		}
	}
}