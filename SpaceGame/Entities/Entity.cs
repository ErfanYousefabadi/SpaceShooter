using System;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Entities;

public abstract class Entity
{
    protected Sprite _sprite;
    public Vector2 Position { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public bool IsActive { get; set; }

    public Entity(Sprite sprite, int maxHP)
    {
        _sprite = sprite;
        MaxHP = maxHP;
        HP = maxHP;
        IsActive = HP > 0;
    }

    public Entity(Sprite sprite, int maxHP, Vector2 position)
    {
        _sprite = sprite;
        MaxHP = maxHP;
        HP = maxHP;
        IsActive = HP > 0;
        Position = position;
    }

    public virtual void Draw()
    {
        _sprite.Draw(Core.SpriteBatch, Position);
    }

    public virtual Circle GetBounds()
    {
        return new Circle(
            (int)(Position.X + _sprite.Width * 0.5f),
            (int)(Position.Y + _sprite.Height * 0.5f),
            (int)(_sprite.Width * 0.5f)
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