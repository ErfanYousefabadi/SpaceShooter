using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using SpaceGame.Entities;

namespace SpaceGame.Enemies;

public class ShooterEnemy : Enemy
{
    private const int SPRITE_SIZE = 52;
    private const int BASE_HP = 30;
    private const int BASE_SPEED = 80;
    private const int CONTACT_DAMAGE = 15;
    private const int SCORE_VALUE = 100;
    private const float COIN_DROP = 0.5f;
    private const int BULLET_SPEED = 300;
    private const int BULLET_DAMAGE = 8;

    private TimeSpan _fireRate = TimeSpan.FromMilliseconds(1500);
    private TimeSpan _timeSinceLastShot = TimeSpan.Zero;

    public ShooterEnemy(Sprite sprite, Vector2 pos) 
        : base(sprite, pos, BASE_SPEED, BASE_HP, SCORE_VALUE, COIN_DROP, null, CONTACT_DAMAGE)
    {
        CoinDropType = CoinType.Silver;
        _sprite.Scale = Vector2.One * SPRITE_SIZE / _sprite.Region.Width;
    }

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
        Bullet b = new(bulletSprite, pos, BULLET_SPEED, Vector2.UnitY, BulletOwner.Enemy, BULLET_DAMAGE);
        activeBullets.Add(b);
    }
}