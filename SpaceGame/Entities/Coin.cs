using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Entities;

public enum CoinType { Silver = 1, Gold = 5}

public class Coin : Entity
{
    public CoinType Type { get; set; }
    public int Value { get; set; }
    const int FALLING_SPEED = 200;

    public Coin(Sprite sprite, Vector2 pos, CoinType type, int? value) : base(sprite, 1, pos)
    {
        Type = type;
        Value = value??((int)type);
    }

    public override void Update(GameTime gameTime)
    {
        Vector2 newPos = Position;
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        newPos.Y += FALLING_SPEED * deltaTime;
        Position = newPos;

        base.Update(gameTime);
    }
}