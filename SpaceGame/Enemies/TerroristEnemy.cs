using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using SpaceGame.Entities;

namespace SpaceGame.Enemies;

public class TerroristEnemy : Enemy
{
    public TerroristEnemy(Sprite sprite, Vector2 pos, float baseSpeed, int baseHP, 
        int scoreValue, float coinDropChance, Vector2? target) 
        : base(sprite, pos, baseSpeed, baseHP, scoreValue, coinDropChance, target) {}

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