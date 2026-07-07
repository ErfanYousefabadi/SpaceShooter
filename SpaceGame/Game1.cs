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
    public Game1() : base("Space Game", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        Texture2D x = Content.Load<Texture2D>("images/ship");
        Sprite s = new(new(x, 0, 0, 32, 32));
        s.Scale = new(4, 4);
        _ship = new(s, 20, 300);
    }

    protected override void Update(GameTime gameTime)
    {
        var screenBounds = GraphicsDevice.PresentationParameters.Bounds;
        _ship.Update(gameTime, screenBounds);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _ship.Draw();

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}