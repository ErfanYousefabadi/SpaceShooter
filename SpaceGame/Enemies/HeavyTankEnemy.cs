using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using SpaceGame.Entities;

namespace SpaceGame.Enemies;

public class HeavyTankEnemy : Enemy
{
    private TimeSpan _fireRate = TimeSpan.FromMilliseconds(1000);
    private TimeSpan _timeSinceLastShot = TimeSpan.Zero;

    public HeavyTankEnemy(Sprite sprite, Vector2 pos, float baseSpeed, int baseHP, 
        int scoreValue, float coinDropChance, Vector2? target) 
        : base(sprite, pos, baseSpeed, baseHP, scoreValue, coinDropChance, target) {}

    public override void Update(GameTime gameTime, List<Bullet> activeBullets, Sprite bulletSprite)
    {
        base.Update(gameTime, activeBullets, bulletSprite);

        _timeSinceLastShot += gameTime.ElapsedGameTime;
        if (_timeSinceLastShot >= _fireRate)
        {
            _timeSinceLastShot -= _fireRate;
            Shoot(activeBullets, bulletSprite);
        }
    }

    public override void Shoot(List<Bullet> bullets, Sprite bulletSprite)
    {
        Circle bounds = GetBounds();
        for (int i = 0; i < 8; i++)
        {
            float angle = MathHelper.PiOver4 * i;
            Vector2 direction = new((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 position = bounds.Location.ToVector2() + (direction * bounds.Radius);
            Bullet b = new(bulletSprite, position, 400, direction, BulletOwner.Enemy, 20);
            bullets.Add(b);
        }
    }

    public override void Move(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 newPos = Position;

        newPos.Y += deltaTime * Speed;
        Position = newPos;
    }
}