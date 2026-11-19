using Godot;

public partial class FallingObject : Area2D
{
	private const float FallSpeed = 50f;

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
			GameManager.Instance.AddScore();
			QueueFree();
		} else if (body.Name == "Floor")
		{
			GameManager.Instance.GameOver();
			QueueFree();
		}
	}
}