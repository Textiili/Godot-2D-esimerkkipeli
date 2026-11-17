using Godot;

public partial class PlayerController : CharacterBody2D
{
    private float Speed = 80f;
    private AnimatedSprite2D _sprite;

  public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _sprite.Play("idle");
    }

    public override void _PhysicsProcess(double delta)
    {
        float direction = Input.GetAxis("move_left", "move_right");

        Velocity = new Vector2(direction * Speed, 0);

        MoveAndSlide();
        Animate(direction);
    }

    private void Animate(float direction)
    {
        string next = direction switch
        {
            > 0 => "move_right",
            < 0 => "move_left",
            _ => "idle"
        };

        if (_sprite.Animation != next)
            _sprite.Play(next);
    }
}