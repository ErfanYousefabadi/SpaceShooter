using Microsoft.Xna.Framework;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Graphics;
using SpaceGame.Enemies;
using SpaceGame.Entities;
using System.Linq;
using System.Collections.Generic;
using MonoGameLibrary;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SpaceGame.Managers;

namespace SpaceGame.Scenes;

public class GamePlayScene : Scene
{
    private Ship _ship;
    private TextureAtlas _atlas;
    private TextureAtlas _explosionAtlas;
    private Animation _explosionAnimation;
    private List<Bullet> _activeBullets = [];
    private List<Enemy> _activeEnemies = [];
    private List<Explosion> _activeExplosions = [];
    private SpriteFactory _spriteFactory;
    private WaveManager _waveManager;

    public override void Initialize()
    {
        base.Initialize();

        _spriteFactory = new(_atlas);
        _explosionAnimation = _explosionAtlas.GetAnimation("explosion-animation");

        _ship = new(_spriteFactory.CreateShipSprite(), new(100, 100));
        _waveManager = new(_spriteFactory);
        _waveManager.StartWave(1);
    }

    public override void LoadContent()
    {
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _explosionAtlas = TextureAtlas.FromFile(Content, "images/explosion-definition.xml");
    }

    public override void Update(GameTime gameTime)
    {
        var screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;
        _ship.Update(gameTime, screenBounds);

        foreach (var b in _activeBullets)
            b.Update(gameTime);
        foreach (var e in _activeEnemies)
        {
            if (e is not TerroristEnemy)
                e.Update(gameTime, _activeBullets, _spriteFactory.CreateBulletSprite());
            else if (e is TerroristEnemy x)
                x.Update(gameTime, _ship.GetBounds().Location.ToVector2());
        }
        foreach (var e in _activeExplosions)
            e.Update(gameTime);

        if (_waveManager.IsWaveComplete && _waveManager.CurrentWave < 10)
            _waveManager.StartWave(_waveManager.CurrentWave + 1);
        else
            _waveManager.Update(gameTime, _activeEnemies);

        _activeBullets.RemoveAll(b => IsCompletelyOut(b.GetBounds(), screenBounds) || !b.IsActive);
        _activeEnemies.RemoveAll(e => IsOutNotFromTop(e.GetBounds(), screenBounds) || !e.IsActive);
        _activeExplosions.RemoveAll(e => e.IsFinished);

        if (Core.Input.Keyboard.IsKeyDown(Keys.Space))
            _ship.Shoot(_spriteFactory.CreateBulletSprite(), _activeBullets);
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.E))
            foreach (TerroristEnemy e in _activeEnemies.Where(e => e is TerroristEnemy))
                e.Explode(_activeExplosions, _explosionAnimation);
        // if (Core.Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left))
        // {
        //     ScoutEnemy e = new(_enemySprite, Core.Input.Mouse.Position.ToVector2());

        //     e.ApplyWaveScaling(1);

        //     _activeEnemies.Add(e);
        // }
    }

    private bool IsCompletelyOut(Circle x, Rectangle screenBounds)
    {
        return x.Bottom < screenBounds.Top
            || x.Top > screenBounds.Bottom
            || x.Left > screenBounds.Right
            || x.Right < screenBounds.Left;

    }

    private bool IsOutNotFromTop(Circle x, Rectangle screenBounds)
    {
        return x.Top > screenBounds.Bottom
            || x.Left > screenBounds.Right
            || x.Right < screenBounds.Left;
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _ship.Draw();

        foreach (var b in _activeBullets)
            b.Draw();
        foreach (var e in _activeEnemies)
            e.Draw();
        foreach (var e in _activeExplosions)
            e.Draw();

        Core.SpriteBatch.End();

        base.Draw(gameTime);
    }
}