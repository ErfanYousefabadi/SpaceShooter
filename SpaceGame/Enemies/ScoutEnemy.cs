using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Enemies;

public class ScoutEnemy : Enemy
{
    private const int SPRITE_SIZE = 40;
    private const int BASE_HP = 15;
    private const int BASE_SPEED = 160;
    private const int CONTACT_DAMAGE = 10;
    private const int SCORE_VALUE = 75;
    private const float COIN_DROP = 0.35f;

    private float _amplitude;
    private float _frequency;
    private TimeSpan _totalTime = TimeSpan.Zero;

    public ScoutEnemy(Sprite sprite, Vector2 pos, float amplitude, float freq) 
        : base(sprite, pos, BASE_SPEED, BASE_HP, SCORE_VALUE, COIN_DROP, null, CONTACT_DAMAGE)
    {
        _amplitude = amplitude;
        _frequency = freq;

        _sprite.Scale = Vector2.One * SPRITE_SIZE / _sprite.Region.Width;
        CoinDropType = Entities.CoinType.Silver;
    }

    public override void Move(GameTime gameTime)
    {
        var newPos = Position;
        _totalTime += gameTime.ElapsedGameTime;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        newPos.X += (float)Math.Sin(_totalTime.TotalSeconds * _frequency) * _amplitude;
        newPos.Y += deltaTime * Speed;

        Position = newPos;
    }
}