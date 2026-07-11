using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using SpaceGame.Enemies;
using SpaceGame.Entities;

namespace SpaceGame.Managers;

public class GameManager
{
    private Ship _ship;
    private List<Enemy> _activeEnemies = [];
    private List<Bullet> _activeBullets = [];
    private List<Explosion> _activeExplosions = [];
    private List<Coin> _activeCoins = [];
    private SpriteFactory _spriteFactory;
    private WaveManager _waveManager;
    private Rectangle _screenBounds;
    private Random _rng;

    public bool IsGameOver { get; private set; } = false;
    public bool IsVictory { get; private set; } = false;

    public GameManager(Rectangle screenBounds, SpriteFactory spriteFactory)
    {
        _rng = new Random();
        _screenBounds = screenBounds;
        _spriteFactory = spriteFactory;
        _waveManager = new(_spriteFactory);
        _ship = new(_spriteFactory.CreateShipSprite(), screenBounds.Center.ToVector2());
        _waveManager.WaveCompleted += OnWaveCompleted;
    }

    public void Update(GameTime gameTime)
    {
        _ship.Update(gameTime, _screenBounds);
        _activeBullets.ForEach(b => b.Update(gameTime));
        _activeCoins.ForEach(c => c.Update(gameTime));
        _activeEnemies.ForEach(e => e.Update(gameTime, _activeBullets, _spriteFactory.CreateBulletSprite()));
        _activeExplosions.ForEach(e => e.Update(gameTime));

        UpdateCollisions();
        UpdateEntityTargets();
        CleanUpInactiveEntities();
        CheckPickUps();
    }

    public void Draw()
    {
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _ship.Draw();

        foreach (var c in _activeCoins)
            c.Draw();
        foreach (var b in _activeBullets)
            b.Draw();
        foreach (var e in _activeEnemies)
            e.Draw();
        foreach (var e in _activeExplosions)
            e.Draw();

        Core.SpriteBatch.End();
    }

    public void Reset()
    {
        _activeCoins = [];
        _activeEnemies = [];
        _activeBullets = [];
        _activeExplosions = [];
        _waveManager.StartWave(1);
        _ship = new(
            _spriteFactory.CreateShipSprite(),
            _screenBounds.Center.ToVector2()
        );
        IsGameOver = IsVictory = false;
    }

    private void UpdateCollisions()
    {
        Circle playerBounds = _ship.GetBounds();
        // player bullets
        foreach (Bullet b in _activeBullets)
        {
            if (b.Owner == BulletOwner.Player) 
                continue;
            if (playerBounds.Intersects(b.GetBounds()))
            {
                b.IsActive = false;
                _ship.TakeDamage(b.Damage);
            }
        }
        // player enemies
        foreach (Enemy e in _activeEnemies)
        {
            if (!playerBounds.Intersects(e.GetBounds())) 
                continue;
            e.IsActive = false;
            _ship.TakeDamage(e.ContactDamage);
            if (e is TerroristEnemy terrorist)
                terrorist.Explode(_activeExplosions, _spriteFactory.ExplosionAnimation);
        }
        // enemies bullets
        foreach (Bullet b in _activeBullets)
        {
            if (b.Owner == BulletOwner.Enemy) 
                continue;

            Circle bulletBounds = b.GetBounds();
            foreach (Enemy e in _activeEnemies)
            {
                if (e.GetBounds().Intersects(bulletBounds))
                {
                    e.TakeDamage(b.Damage);
                    b.IsActive = false;
                    _ship.Score += e.ScoreValue;
                }
            }
        }
    }

    private void UpdateEntityTargets()
    {
        List<TerroristEnemy> terrorists = _activeEnemies
            .Where(e => e is TerroristEnemy)
            .Cast<TerroristEnemy>()
            .ToList();
        Vector2 newTarget = _ship.GetBounds().Location.ToVector2();
        foreach (TerroristEnemy t in terrorists)
            t.Target = newTarget;
    }

    private void CleanUpInactiveEntities()
    {
        if (_ship.IsActive == false) Environment.Exit(0);
        _activeBullets.RemoveAll(b => IsCompletelyOut(b.GetBounds(), _screenBounds) || !b.IsActive);
        _activeExplosions.RemoveAll(e => e.IsFinished);
        _activeCoins.RemoveAll(c => !c.IsActive);
        foreach(var enemy in _activeEnemies.Where(e => e.HP == 0))
            HandleEnemyDestroyed(enemy);
        _activeEnemies.RemoveAll(e => IsOutNotFromTop(e.GetBounds(), _screenBounds) || !e.IsActive);
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        if (_rng.NextDouble() < enemy.CoinDropChance)
            SpawnCoin(enemy.Position, enemy.CoinDropType);
    }

    private void SpawnCoin(Vector2 position, CoinType type)
    {
        // TODO: needs coin sprite to be added
    }

    private void CheckPickUps() // coins and perhaps powerups in future
    {
        Circle playerBounds = _ship.GetBounds();
        foreach(var c in _activeCoins)
        {
            if (!playerBounds.Intersects(c.GetBounds())) 
                continue;
            c.IsActive = false;
            _ship.Coins += (int)c.Type;
        }
    }

    public void OnWaveCompleted(int waveNumber)
    {
        _ship.Score += 100 * waveNumber;
        _ship.Coins += 5 * waveNumber;
    }

    private static bool IsCompletelyOut(Circle x, Rectangle screenBounds)
    {
        return x.Bottom < screenBounds.Top
            || x.Top > screenBounds.Bottom
            || x.Left > screenBounds.Right
            || x.Right < screenBounds.Left;

    }

    private static bool IsOutNotFromTop(Circle x, Rectangle screenBounds)
    {
        return x.Top > screenBounds.Bottom
            || x.Left > screenBounds.Right
            || x.Right < screenBounds.Left;
    }
}