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

namespace SpaceGame.Scenes;

public class GamePlayScene : Scene
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

    public override void Initialize()
    {
        base.Initialize();

        Sprite shipSprite = new(_atlas.GetRegion("ship"));

        _bulletSprite = new(_atlas.GetRegion("bullet"));

        _enemySprite = new(_atlas.GetRegion("ship"));
        _enemySprite.CenterOrigin();
        _enemySprite.Rotation = MathHelper.Pi;

        _explosionAnimation = _explosionAtlas.GetAnimation("explosion-animation");

        _ship = new(shipSprite, new(100, 100));
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
                e.Update(gameTime, _activeBullets, _bulletSprite);
            else if (e is TerroristEnemy x)
                x.Update(gameTime, _ship.GetBounds().Location.ToVector2());
        }
        foreach (var e in _activeExplosions)
            e.Update(gameTime);

        _activeBullets.RemoveAll(b => IsCompletelyOut(b.GetBounds(), screenBounds) || !b.IsActive);
        _activeEnemies.RemoveAll(e => IsCompletelyOut(e.GetBounds(), screenBounds) || !e.IsActive);
        _activeExplosions.RemoveAll(e => e.IsFinished);
        
        if (Core.Input.Keyboard.IsKeyDown(Keys.Space))
            _ship.Shoot(_bulletSprite, _activeBullets);
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.E))
            foreach (TerroristEnemy e in _activeEnemies.Where(e => e is TerroristEnemy))
                e.Explode(_activeExplosions, _explosionAnimation);

        if (Core.Input.Mouse.WasButtonJustPressed(MonoGameLibrary.Input.MouseButton.Left))
        {
            TerroristEnemy e = new(_enemySprite, Core.Input.Mouse.Position.ToVector2(), 100, 30, 30, 1, _ship.Position);

            e.ApplyWaveScaling(10);

            _activeEnemies.Add(e);
        }
    }

    private bool IsCompletelyOut (Circle x, Rectangle screenBounds)
    {
        return x.Bottom < screenBounds.Top
            || x.Top > screenBounds.Bottom
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