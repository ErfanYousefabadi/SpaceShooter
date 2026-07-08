using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using SpaceGame.Entities;

namespace SpaceGame.Enemies;

public class ScoutEnemy : Enemy
{
    private float _amplitude;
    private float _frequency;

    public ScoutEnemy(Sprite sprite, Vector2 pos, float baseSpeed, int baseHP, 
        int scoreValue, float coinDropChance, Vector2? target, float amplitude, float freq) 
        : base(sprite, pos, baseSpeed, baseHP, scoreValue, coinDropChance, target)
    {
        _amplitude = amplitude;
        _frequency = freq;
    }

    public ScoutEnemy(AnimatedSprite sprite, Vector2 pos, float baseSpeed, int baseHP, 
        int scoreValue, float coinDropChance, Vector2? target, float amplitude, float freq) 
        : base(sprite, pos, baseSpeed, baseHP, scoreValue, coinDropChance, target)
    {
        _amplitude = amplitude;
        _frequency = freq;
    }

    public override void Move(GameTime gameTime)
    {
        var newPos = Position;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        newPos.X += (float)Math.Sin(deltaTime * _frequency) * _amplitude;
        newPos.Y += deltaTime * Speed;

        Position = newPos;
    }
}