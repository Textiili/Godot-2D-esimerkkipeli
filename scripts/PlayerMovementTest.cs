using Godot;

public partial class PlayerMovementTest : CharacterBody2D
{
    [Export] private float Speed = 100f;

    public override void _PhysicsProcess(double delta)
    {
        float direction = Input.GetAxis("move_left", "move_right");

        Velocity = new Vector2(direction * Speed, 0);

        MoveAndSlide();
    }
}