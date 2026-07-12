using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using SpaceGame.Enemies;
using SpaceGame.Entities;

namespace SpaceGame.Managers;

public class GameManager
{
    private Ship _ship;
    private SpriteFont _font;
    private List<Enemy> _activeEnemies = [];
    private List<Bullet> _activeBullets = [];
    private List<Explosion> _activeExplosions = [];
    private List<Coin> _activeCoins = [];
    private SpriteFactory _spriteFactory;
    private WaveManager _waveManager;
    private Rectangle _screenBounds;
    private Random _rng;
    private Texture2D _endingScreenshot = null;

    public bool IsGameOver { get; private set; } = false;
    public bool IsVictory { get; private set; } = false;
    public int Score => _ship.Score;
    public int Coins => _ship.Coins;
    public Texture2D EndingScreenshot => _endingScreenshot;

    public GameManager(Rectangle screenBounds, SpriteFactory spriteFactory, SpriteFont font)
    {
        _font = font;
        _rng = new Random();
        _screenBounds = screenBounds;
        _spriteFactory = spriteFactory;
        _waveManager = new(_spriteFactory);
        _waveManager.StartWave(1);
        _ship = new(_spriteFactory.CreateShipSprite(), screenBounds.Center.ToVector2());
        _waveManager.WaveCompleted += OnWaveCompleted;
    }

    public void Update(GameTime gameTime)
    {
        if (_ship.IsActive)
            _ship.Update(gameTime, _screenBounds);
        _activeBullets.ForEach(b => b.Update(gameTime));
        _activeCoins.ForEach(c => c.Update(gameTime));
        _activeEnemies.ForEach(e => e.Update(gameTime, _activeBullets, _spriteFactory.CreateBulletSprite()));
        _activeExplosions.ForEach(e => e.Update(gameTime));

        if (Core.Input.Keyboard.IsKeyDown(Keys.Space))
            _ship.Shoot(_spriteFactory.CreateBulletSprite(), _activeBullets);

        UpdateCollisions();
        UpdateEntityTargets();
        CleanUpInactiveEntities();
        CheckPickUps();

        _waveManager.Update(gameTime, _activeEnemies);
        if (_ship.HP == 0)
        {
            IsGameOver = true;
            IsVictory = false;
        }
        if (_waveManager.CurrentWave == 10 && _waveManager.IsWaveComplete == true && _ship.HP > 0)
        {
            IsGameOver = true;
            IsVictory = true;
        }
        if (IsGameOver && _endingScreenshot == null)
        {
            _endingScreenshot = CaptureFinalFrame(Core.GraphicsDevice);
        }
    }

    public void Draw()
    {
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        if (_ship.IsActive)
            _ship.Draw();

        foreach (var c in _activeCoins)
            c.Draw();
        foreach (var b in _activeBullets)
            b.Draw();
        foreach (var e in _activeEnemies)
            e.Draw();
        foreach (var e in _activeExplosions)
            e.Draw();

        Core.SpriteBatch.DrawString(
            _font, $"Score: {_ship.Score} / Coins: {_ship.Coins} / Wave: {_waveManager.CurrentWave}"
            , new(5, 5), Color.Black
        );
        string shp = $"HP: {_ship.HP}";
        var s = _font.MeasureString(shp);
        Core.SpriteBatch.DrawString(
            _font, shp, new((_screenBounds.Width - s.X) * 0.5f, _screenBounds.Height - s.Y), Color.Black
        );

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
        // if (_ship.IsActive == false) Environment.Exit(0);
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
        _ship.Score += enemy.ScoreValue;
    }

    private void SpawnCoin(Vector2 position, CoinType type)
    {
        Coin c;
        if (type == CoinType.Silver)
            c = new(_spriteFactory.CreateSilverCoinSprite(), position, CoinType.Silver, null);
        else
            c = new(_spriteFactory.CreateGoldCoinSprite(), position, CoinType.Gold, null);

        _activeCoins.Add(c);
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

        if (_waveManager.CurrentWave < 10)
            _waveManager.StartWave(_waveManager.CurrentWave + 1);
    }

    private Texture2D CaptureFinalFrame(GraphicsDevice graphicsDevice)
    {
        int[] backBuffer = new int[_screenBounds.Width * _screenBounds.Height];
        graphicsDevice.GetBackBufferData(backBuffer);

        //copy into a texture 
        Texture2D texture = new Texture2D(
            graphicsDevice, 
            _screenBounds.Width, 
            _screenBounds.Height, 
            false, 
            graphicsDevice.PresentationParameters.BackBufferFormat
        );
        texture.SetData(backBuffer);

        return texture;
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