using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Entities;

public enum BulletOwner { Player, Enemy }

public class Bullet : Entity
{
    private Vector2 _velocity;
    private Vector2 _direction;

    public BulletOwner Owner { get; set; }
    public int Damage { get; set; }

    public Bullet(Sprite sprite, Vector2 pos, float speed, Vector2 direction, 
        BulletOwner bulletOwner, int damage) : base(sprite, 1, pos, speed)
    {
        _direction = direction;
        _velocity = direction * speed;
        Owner = bulletOwner;
        Damage = damage;
    }

    public override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += _velocity * deltaTime;

        base.Update(gameTime);
    }
    
    public bool IsOffScreen(Rectangle roomBounds)
        => !roomBounds.Contains(Position.ToPoint());
}