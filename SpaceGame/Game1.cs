using System;
using System.Collections.Generic;
using System.Linq;
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

        foreach (var b in _activeBullets)
            b.Update(gameTime);
        foreach (var e in _activeEnemies)
        {
            if (e is not TerroristEnemy)
                e.Update(gameTime, _activeBullets, _bulletSprite);
            else if (e is TerroristEnemy x)
                x.Update(gameTime, _ship.GetBounds().Location.ToVector2());
        }
        foreach (var e in _activeExplosions)
            e.Update(gameTime);

        _activeBullets.RemoveAll(b => IsCompletelyOut(b.GetBounds(), screenBounds) || !b.IsActive);
        _activeEnemies.RemoveAll(e => IsCompletelyOut(e.GetBounds(), screenBounds) || !e.IsActive);
        _activeExplosions.RemoveAll(e => e.IsFinished);
        
        if (Input.Keyboard.IsKeyDown(Keys.Space))
            _ship.Shoot(_bulletSprite, _activeBullets);
        if (Input.Keyboard.WasKeyJustPressed(Keys.E))
            foreach (TerroristEnemy e in _activeEnemies.Where(e => e is TerroristEnemy))
                e.Explode(_activeExplosions, _explosionAnimation);

        if (Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left))
        {
            TerroristEnemy e = new(_enemySprite, Input.Mouse.Position.ToVector2(), 100, 30, 30, 1, _ship.Position);

            e.ApplyWaveScaling(10);

            _activeEnemies.Add(e);
        }

        base.Update(gameTime);
    }

    private static bool IsCompletelyOut (Circle x, Rectangle screenBounds)
    {
        return x.Bottom < screenBounds.Top
            || x.Top > screenBounds.Bottom
            || x.Left > screenBounds.Right
            || x.Right < screenBounds.Left;

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