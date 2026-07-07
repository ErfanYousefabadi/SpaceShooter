using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Entities;

public enum SpriteType { Normal, Animated }

public abstract class Entity
{
    protected Sprite _sprite;
    protected AnimatedSprite _animSprite;
    public Vector2 Position { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public bool IsActive { get; set; }
    public SpriteType SpriteType { get; }

    public Entity(Sprite sprite, int maxHP, Vector2 position)
    {
        _sprite = sprite;
        MaxHP = maxHP;
        HP = maxHP;
        IsActive = HP > 0;
        Position = position;
        SpriteType = SpriteType.Normal;
    }

    public Entity(AnimatedSprite sprite, int maxHP, Vector2 position)
    {
        _animSprite = sprite;
        MaxHP = maxHP;
        HP = maxHP;
        IsActive = HP > 0;
        Position = position;
        SpriteType = SpriteType.Animated;
    }

    public virtual void Draw()
    {
        if (SpriteType == SpriteType.Normal)
            _sprite.Draw(Core.SpriteBatch, Position);
        else 
            _animSprite.Draw(Core.SpriteBatch, Position);
    }

    public virtual Circle GetBounds()
    {
        if (SpriteType == SpriteType.Normal)
            return new Circle(
                (int)(Position.X + _sprite.Width * 0.5f),
                (int)(Position.Y + _sprite.Height * 0.5f),
                (int)Math.Max(_sprite.Width * 0.5f, _sprite.Height * 0.5f)
            );
        else    
            return new Circle(
                (int)(Position.X + _animSprite.Width * 0.5f),
                (int)(Position.Y + _animSprite.Height * 0.5f),
                (int)Math.Max(_animSprite.Width * 0.5f, _animSprite.Height * 0.5f)
            );
    }

    public virtual void TakeDamage(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException("amount must be positive.");
        HP = Math.Max(0, HP - amount);
        if (HP == 0) IsActive = false;
    }

    protected virtual void OnDestroyed() {}
}