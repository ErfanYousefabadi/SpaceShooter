using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using SpaceGame.Enemies;
using SpaceGame.Entities;

namespace SpaceGame;

public class Game1 : Core
{
    private Ship _ship;
    private Sprite _bulletSprite;
    private Sprite _enemySprite;
    private Animation _explosionAnimation;
    private TextureAtlas _atlas;
    private TextureAtlas _explosionAtlas;
    private List<Bullet> _activeBullets = [];
    private List<Enemy> _activeEnemies = [];
    private List<Explosion> _activeExplosions = [];

    public Game1() : base("Space Game", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
        Sprite shipSprite = new(_atlas.GetRegion("ship"));
        _bulletSprite = new(_atlas.GetRegion("bullet"));
        _bulletSprite.Scale = new(1.5f, 1.5f);
        shipSprite.Scale = new(2, 2);
        _ship = new(shipSprite, 20, new(100, 100), 300);
        _enemySprite = new(_atlas.GetRegion("ship"));
        _enemySprite.Origin = new Vector2(_enemySprite.Width, _enemySprite.Height) * 0.5f;
        _enemySprite.Rotation = MathHelper.Pi;
        _explosionAnimation = _explosionAtlas.GetAnimation("explosion-animation");
    }

    protected override void LoadContent()
    {
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _explosionAtlas = TextureAtlas.FromFile(Content, "images/explosion-definition.xml");
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        var screenBounds = GraphicsDevice.PresentationParameters.Bounds;
        _ship.Update(gameTime, screenBounds);

        List<Bullet> bulletRemove = [];
        List<Enemy> enemyRemove = [];
        List<Explosion> explosionRemove = [];
        foreach (var b in _activeBullets)
        {
            var bbounds = b.GetBounds();
            b.Update(gameTime);
            if (bbounds.Bottom < screenBounds.Top
                || bbounds.Top > screenBounds.Bottom
                || bbounds.Left > screenBounds.Right
                || bbounds.Right < screenBounds.Left)
                bulletRemove.Add(b);
        }
        foreach (var e in _activeEnemies)
        {
            var ebounds = e.GetBounds();
            e.Update(gameTime, _activeBullets, _bulletSprite);
            if (ebounds.Bottom < screenBounds.Top
                || ebounds.Top > screenBounds.Bottom
                || ebounds.Left > screenBounds.Right
                || ebounds.Right < screenBounds.Left)
                enemyRemove.Add(e);
        }

        foreach (var e in _activeExplosions)
        {
            e.Update(gameTime);
            if (e.IsFinished)
            {
                explosionRemove.Add(e);
            }
        }

        foreach(var b in bulletRemove)
            _activeBullets.Remove(b);
        foreach(var e in enemyRemove)
            _activeEnemies.Remove(e);
        foreach(var e in explosionRemove)
            _activeExplosions.Remove(e);
        
        if (Input.Keyboard.IsKeyDown(Keys.Space))
            _ship.Shoot(_bulletSprite, _activeBullets);

        // if (Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left))
        // {
        //     ScoutEnemy e = new(_enemySprite, Input.Mouse.Position.ToVector2(), 100, 30, 30, 1, null, 5, 10);

        //     e.ApplyWaveScaling(1);

        //     _activeEnemies.Add(e);
        // }

        if (Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left))
        {
            AnimatedSprite sprite = new(_explosionAnimation);
            sprite.Scale = new(4, 4);
            Explosion e = new(sprite, Input.Mouse.Position.ToVector2());
            _activeExplosions.Add(e);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _ship.Draw();

        foreach (var b in _activeBullets)
            b.Draw();
        foreach (var e in _activeEnemies)
            e.Draw();
        foreach (var e in _activeExplosions)
            e.Draw();

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}