using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    private TextureAtlas _atlas;
    private List<Bullet> _activeBullets = [];
    private List<Enemy> _activeEnemies = [];

    public Game1() : base("Space Game", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
        Sprite shipSprite = new(_atlas.GetRegion("ship"));
        _bulletSprite = new(_atlas.GetRegion("bullet"));
        _bulletSprite.Scale = new(1, 1);
        shipSprite.Scale = new(2, 2);
        _ship = new(shipSprite, 20, new(100, 100), 300);
        _enemySprite = new(_atlas.GetRegion("ship"));
        _enemySprite.Origin = new Vector2(_enemySprite.Width, _enemySprite.Height) * 0.5f;
        _enemySprite.Rotation = MathHelper.Pi;
    }

    protected override void LoadContent()
    {
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        var screenBounds = GraphicsDevice.PresentationParameters.Bounds;
        _ship.Update(gameTime, screenBounds);

        List<Bullet> bremove = [];
        List<Enemy> eremove = [];
        foreach (var b in _activeBullets)
        {
            b.Update(gameTime);
            if (b.GetBounds().Bottom < screenBounds.Top)
                bremove.Add(b);
        }
        foreach (var e in _activeEnemies)
        {
            e.Update(gameTime, _activeBullets, _bulletSprite);
            if (e.GetBounds().Top > screenBounds.Bottom)
                eremove.Add(e);
        }

        foreach(var b in bremove)
            _activeBullets.Remove(b);
        foreach(var e in eremove)
            _activeEnemies.Remove(e);
        
        if (Input.Keyboard.IsKeyDown(Keys.Space))
            _ship.Shoot(_bulletSprite, _activeBullets);

        if (Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left))
        {
            ShooterEnemy e = new(_enemySprite, Input.Mouse.Position.ToVector2(), 100, 30, 30, 1, null);

            e.ApplyWaveScaling(1);

            _activeEnemies.Add(e);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (var b in _activeBullets)
            b.Draw();
        foreach (var e in _activeEnemies)
            e.Draw();

        _ship.Draw();

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}