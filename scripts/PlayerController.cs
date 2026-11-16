using Godot;

public partial class PlayerController : CharacterBody2D
{
    private float Speed = 150f;

    public override void _PhysicsProcess(double delta)
    {
        float direction = Input.GetAxis("move_left", "move_right");

        Velocity = new Vector2(direction * Speed, 0);

        MoveAndSlide();
        //TODO: Animate!
    }
}