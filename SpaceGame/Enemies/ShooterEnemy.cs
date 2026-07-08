using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using SpaceGame.Entities;

namespace SpaceGame.Enemies;

public class ShooterEnemy : Enemy
{
    private TimeSpan _fireRate = TimeSpan.FromMilliseconds(1500);
    private TimeSpan _timeSinceLastShot = TimeSpan.Zero;

    public ShooterEnemy(Sprite sprite, Vector2 pos, float baseSpeed, int baseHP, 
        int scoreValue, float coinDropChance, Vector2? target) 
        : base(sprite, pos, baseSpeed, baseHP, scoreValue, coinDropChance, target) {}

    public ShooterEnemy(AnimatedSprite sprite, Vector2 pos, float baseSpeed, int baseHP, 
        int scoreValue, float coinDropChance, Vector2? target) 
        : base(sprite, pos, baseSpeed, baseHP, scoreValue, coinDropChance, target) {}

    public override void Move(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var newPos = Position;

        newPos.Y += deltaTime * Speed;

        Position = newPos;
    }

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

    public override void Shoot(List<Bullet> activeBullets, Sprite bulletSprite) 
    {
        Circle bounds = GetBounds();
        Vector2 pos = new(bounds.Location.X, bounds.Bottom);
        Bullet b = new(bulletSprite, pos, 400, Vector2.UnitY, BulletOwner.Enemy, 20);
        activeBullets.Add(b);
    }
}