using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using SpaceGame.Entities;

public class Ship : Entity
{
    private static TimeSpan _fireRate = TimeSpan.FromMilliseconds(200);
    private const int _bulletSpeed = 600;
    private const int _maxhp = 100;
    private const int _bulletDamage = 10;
    private const int _shipSpeed = 350;

    private TimeSpan _timeSinceLastShot = _fireRate;

    public int Score { get; set; }
    public int Coins { get; set; }

    public Ship(Sprite sprite, Vector2 pos)
        : base(sprite, _maxhp, pos, _shipSpeed)
    {
        _sprite.Scale = Vector2.One * 64 / _sprite.Region.Width;
        // makes the texture 64 * 64
        Score = 0;
        Coins = 0;
    }

    private void Move(float deltaTime, Rectangle roomBounds)
    {
        Vector2 delta = Vector2.Zero;
        if (Core.Input.Keyboard.IsKeyDown(Keys.W))
            delta.Y -= Speed;
        if (Core.Input.Keyboard.IsKeyDown(Keys.S))
            delta.Y += Speed;
        if (Core.Input.Keyboard.IsKeyDown(Keys.A))
            delta.X -= Speed;
        if (Core.Input.Keyboard.IsKeyDown(Keys.D))
            delta.X += Speed;

        delta *= deltaTime;

        Position += delta;

        var shipBound = GetBounds();
        Vector2 newPos = Position;

        if (shipBound.Top < roomBounds.Top)
            newPos.Y = shipBound.Radius;
        if (shipBound.Bottom > roomBounds.Bottom)
            newPos.Y = roomBounds.Bottom - shipBound.Radius;
        if (shipBound.Left < roomBounds.Left)
            newPos.X = shipBound.Radius;
        if (shipBound.Right > roomBounds.Right)
            newPos.X = roomBounds.Right - shipBound.Radius;

        Position = newPos;
    }

    public void Shoot(Sprite bs, List<Bullet> activeBullets, SoundEffect se)
    {
        if (_timeSinceLastShot < _fireRate)
            return;
        Vector2 pos = new(GetBounds().Location.X, GetBounds().Top);
        Bullet bullet = new(
            bs, pos,
            _bulletSpeed,
            -Vector2.UnitY,
            BulletOwner.Player,
            _bulletDamage
        );
        _timeSinceLastShot = TimeSpan.Zero;
        activeBullets.Add(bullet);
        Core.Audio.PlaySoundEffect(se, 0.1f, 1, 0, false);
    }

    public void Update(GameTime gameTime, Rectangle roomBounds)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _timeSinceLastShot += gameTime.ElapsedGameTime;

        Move(deltaTime, roomBounds);
    }
}