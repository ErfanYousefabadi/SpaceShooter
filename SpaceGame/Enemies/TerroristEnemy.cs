using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using SpaceGame.Entities;

namespace SpaceGame.Enemies;

public class TerroristEnemy : Enemy
{
    private const int SPRITE_SIZE = 44;
    private const int BASE_HP = 10;
    private const int BASE_SPEED = 200;
    private const int CONTACT_DAMAGE = 30;
    private const int SCORE_VALUE = 125;
    private const float COIN_DROP = 0.3f;

    public TerroristEnemy(Sprite sprite, Vector2 pos, Vector2? target) 
        : base(sprite, pos, BASE_SPEED, BASE_HP, SCORE_VALUE, COIN_DROP, target, CONTACT_DAMAGE)
    {
        CoinDropType = CoinType.Silver;
        _sprite.Scale = Vector2.One * SPRITE_SIZE / _sprite.Region.Width;
    }

    public override void Move(GameTime gameTime)
    {
        if (Target == null) return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        Vector2 dir = (Vector2)Target - GetBounds().Location.ToVector2();
        if (dir.Length() < 2) return;
        dir.Normalize();

        Position += dir * Speed * deltaTime;
    }

    public void Update(GameTime gameTime, Vector2? target)
    {
        Target = target;
        base.Update(gameTime, null, null);
    }

    public void Explode(List<Explosion> activeExplosions, Animation explosionAnimation)
    {
        AnimatedSprite explosionSprite = new(explosionAnimation);
        explosionSprite.CenterOrigin();
        explosionSprite.Scale = _sprite.Scale;
        Explosion e = new(explosionSprite, GetBounds().Location.ToVector2());
        activeExplosions.Add(e);
        IsActive = false;
    }
}