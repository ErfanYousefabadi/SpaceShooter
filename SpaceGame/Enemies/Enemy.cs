using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;
using SpaceGame.Entities;

namespace SpaceGame.Enemies;

public abstract class Enemy : Entity
{
    protected float _baseSpeed;
    protected int _baseHP;

    public int ScoreValue { get; set; }
    public float CoinDropChance { get; set; }
    public CoinType CoinDropType { get; set; }
    public Vector2? Target { get; set; }

    public Enemy(Sprite sprite, Vector2 pos, float baseSpeed, int baseHP, 
        int scoreValue, float coinDropChance, Vector2? target) 
        : base(sprite, baseHP + 20, pos)
    {
        _baseSpeed = baseSpeed;
        _baseHP = baseHP;
        ScoreValue = scoreValue;
        CoinDropChance = coinDropChance;
        Target = target;
    }

    public abstract void Move(GameTime gameTime);

    public virtual void Update(GameTime gameTime, List<Bullet> activeBullets, Sprite bulletSprite)
    {
        base.Update(gameTime);
        Move(gameTime);
    }

    public virtual void Shoot(List<Bullet> bullets, Sprite bulletSprite) { }

    public virtual void ApplyWaveScaling(int wave)
    {
        Speed = _baseSpeed * (1 + 0.1f * wave);
        HP = _baseHP + 2 * wave;
    }

    protected override void OnDestroyed()
    {
        base.OnDestroyed();
    }
}