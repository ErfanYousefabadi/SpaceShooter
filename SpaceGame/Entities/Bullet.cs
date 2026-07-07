using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Entities;

public enum BulletOwner { Player, Enemy }

public class Bullet : Entity
{
    private Vector2 _velocity;

    public BulletOwner Owner { get; set; }
    public int Damage { get; set; }

    public Bullet(Sprite sprite, Vector2 pos, Vector2 vel, 
        BulletOwner bulletOwner, int damage) : base(sprite, 1, pos)
    {
        _velocity = vel;
        Owner = bulletOwner;
        Damage = damage;
    }

    public void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += _velocity * deltaTime;
    }
    
    public bool IsOffScreen(Rectangle roomBounds)
        => !roomBounds.Contains(Position.ToPoint());
}