using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using SpaceGame.Entities;

public class Ship : Entity
{
    private float _speed; // pixels per second (will be multiplied by deltaTime)
    private TimeSpan _fireRate = TimeSpan.FromMilliseconds(200);
    private TimeSpan _timeSinceLastShot = TimeSpan.FromMilliseconds(200);

    public int Score { get; set; }
    public int Coins { get; set; }

    public Ship(Sprite sprite, int maxHP, Vector2 pos, float speed) 
        : base(sprite, maxHP, pos)
    {
        _speed = speed;
    }

    private void Move(float deltaTime, Rectangle roomBounds)
    {
        Vector2 delta = Vector2.Zero;
        if (Core.Input.Keyboard.IsKeyDown(Keys.W))
            delta.Y -= _speed;
        if (Core.Input.Keyboard.IsKeyDown(Keys.S))
            delta.Y += _speed;
        if (Core.Input.Keyboard.IsKeyDown(Keys.A))
            delta.X -= _speed;
        if (Core.Input.Keyboard.IsKeyDown(Keys.D))
            delta.X += _speed;

        delta *= deltaTime;        

        Position += delta;

        var shipBound = GetBounds();
        Vector2 newPos = Position;

        if (shipBound.Top < roomBounds.Top)
            newPos.Y = 0;
        if (shipBound.Bottom > roomBounds.Bottom)
            newPos.Y = roomBounds.Bottom - _sprite.Height;
        if (shipBound.Left < roomBounds.Left)
            newPos.X = 0;
        if (shipBound.Right > roomBounds.Right)
            newPos.X = roomBounds.Right - _sprite.Width;
        
        Position = newPos;
    }

    public Bullet Shoot(Sprite bs)
    {
        if (_timeSinceLastShot < _fireRate)
            return null;
        Vector2 pos = new(GetBounds().Left + _sprite.Width * 0.5f - bs.Width * 0.5f, GetBounds().Top);
        Bullet bullet = new(bs, pos, new(0, -400), BulletOwner.Player, 20);
        _timeSinceLastShot = TimeSpan.Zero;
        return bullet;
    }    

    public void Update(GameTime gameTime, Rectangle roomBounds)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _timeSinceLastShot += gameTime.ElapsedGameTime;

        Move(deltaTime, roomBounds);
    }
}