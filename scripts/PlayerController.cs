using Godot;

public partial class PlayerController : CharacterBody2D
{
    private float Speed = 80f;
    private AnimatedSprite2D _sprite;

  public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _sprite.Play("idle"); //Varmistetaan, että idle-animaatio käynnistyy 
    }

    public override void _PhysicsProcess(double delta)
    {
        float direction = Input.GetAxis("move_left", "move_right");

        Velocity = new Vector2(direction * Speed, 0);

        MoveAndSlide(); //Kutsutaan Godot Enginen valmiiksi tarjoamaa metodia liikkumiselle. Tämä metodi lukee parent-noden Velocityn ja reagoi siihen.
        Animate(direction); //Kutsutaan animaatio-metodia ja kerrotaan sille, kuljetaanko vasemmalle, oikealle vai ollaanko paikallaan.
    }

    private void Animate(float direction)
    {
        string next = direction switch //Luodaan uusi väliaikainen muuttuja, johon laitetaan string suunnan perusteella
        {
            > 0 => "move_right",
            < 0 => "move_left",
            _ => "idle"
        };

        if (_sprite.Animation != next) //Ei ylikirjoiteta esim. move_left -animaatiota käynnistämällä sitä uudelleen, kun se on aktiivinen
            _sprite.Play(next);
    }
}