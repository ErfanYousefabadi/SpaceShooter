using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using SpaceGame.Entities;

namespace SpaceGame.Enemies;

public class HeavyTankEnemy : Enemy
{
    private const int SPRITE_SIZE = 96;
    private const int BASE_HP = 80;
    private const int BASE_SPEED = 40;
    private const int CONTACT_DAMAGE = 25;
    private const int SCORE_VALUE = 250;
    private const float COIN_DROP = 0.9f;
    private const int BULLET_SPEED = 220;
    private const int BULLET_DAMAGE = 10;

    private TimeSpan _fireRate = TimeSpan.FromMilliseconds(3000);
    private TimeSpan _timeSinceLastShot = TimeSpan.Zero;

    public HeavyTankEnemy(Sprite sprite, Vector2 pos)
        : base(sprite, pos, BASE_SPEED, BASE_HP, SCORE_VALUE, COIN_DROP, null, CONTACT_DAMAGE)
    {
        CoinDropType = CoinType.Gold;
        _sprite.Scale = Vector2.One * SPRITE_SIZE / _sprite.Region.Width;
    }

    public override void Update(GameTime gameTime, List<Bullet> activeBullets, TextureRegion bulletRegion)
    {
        base.Update(gameTime, activeBullets, bulletRegion);

        _timeSinceLastShot += gameTime.ElapsedGameTime;
        if (_timeSinceLastShot >= _fireRate)
        {
            _timeSinceLastShot -= _fireRate;
            Shoot(activeBullets, bulletRegion);
        }
    }

    public override void Shoot(List<Bullet> bullets, TextureRegion bulletRegion)
    {
        Circle bounds = GetBounds();
        for (int i = 0; i < 8; i++)
        {
            float angle = MathHelper.PiOver4 * i;
            Sprite bulletSprite = new(bulletRegion);
            bulletSprite.CenterOrigin();
            bulletSprite.Scale = new(0.5f, 0.5f);
            bulletSprite.Rotation = angle + MathHelper.PiOver2;
            Vector2 direction = new((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 position = bounds.Location.ToVector2() + (direction * bounds.Radius);
            Bullet b = new(bulletSprite, position, BULLET_SPEED, direction, BulletOwner.Enemy, BULLET_DAMAGE);
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