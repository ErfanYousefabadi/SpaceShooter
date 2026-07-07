using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using SpaceGame.Entities;

namespace SpaceGame;

public class Game1 : Core
{
    private Ship _ship;
    private Sprite _bulletSprite;
    private TextureAtlas _atlas;
    private List<Bullet> _activeBullets = [];

    public Game1() : base("Space Game", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
        Sprite shipSprite = new(_atlas.GetRegion("ship"));
        _bulletSprite = new(_atlas.GetRegion("bullet"));
        _bulletSprite.Scale = new(3, 3);
        shipSprite.Scale = new(4, 4);
        _ship = new(shipSprite, 20, new(100, 100), 300);
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

        List<Bullet> remove = [];
        foreach (var b in _activeBullets)
        {
            b.Update(gameTime);
            if (b.GetBounds().Bottom < screenBounds.Top)
                remove.Add(b);
        }
        foreach(var b in remove)
            _activeBullets.Remove(b);
        
        if (Input.Keyboard.IsKeyDown(Keys.Space))
        {
            var x = _ship.Shoot(_bulletSprite);
            if (x != null) _activeBullets.Add(x);
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (var b in _activeBullets)
            b.Draw();

        _ship.Draw();

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}