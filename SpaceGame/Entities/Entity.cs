using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Entities;

public enum SpriteType { Normal, Animated }

public abstract class Entity
{
    protected Sprite _sprite;
    public Vector2 Position { get; set; }
    public int HP { get; set; }
    public float Speed { get; set; } // pixels per second
    public int MaxHP { get; set; }
    public bool IsActive { get; set; }
    public SpriteType SpriteType { get; }

    public Entity(Sprite sprite, int maxHP, Vector2 position, float speed = 0)
    {
        _sprite = sprite;
        Speed = speed;
        MaxHP = maxHP;
        HP = maxHP;
        IsActive = HP > 0;
        Position = position;
        if (_sprite is AnimatedSprite)
            SpriteType = SpriteType.Animated;
        else
            SpriteType = SpriteType.Normal;
    }

    public virtual void Draw()
    {
        _sprite.Draw(Core.SpriteBatch, Position);
    }

    public virtual void Update(GameTime gameTime)
    {
        if (_sprite is AnimatedSprite a)
            a.UpdateAnimetion(gameTime);
    }

    public virtual Circle GetBounds()
    {
        Vector2 center = Position - _sprite.Origin * _sprite.Scale + new Vector2(_sprite.Width, _sprite.Height) * 0.5f;

        return new Circle(
            (int)center.X,
            (int)center.Y,
            (int)(Math.Max(_sprite.Width, _sprite.Height) * 0.5f)
        );
    }

    public virtual void TakeDamage(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException("amount must be positive.");
        HP = Math.Max(0, HP - amount);
        if (HP == 0) IsActive = false;
    }

    protected virtual void OnDestroyed() { }
}