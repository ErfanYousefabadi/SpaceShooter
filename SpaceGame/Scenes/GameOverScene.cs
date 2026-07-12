using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;

namespace SpaceGame.Scenes;

public class GameOverScene : Scene
{
    private int _finalScore;
    private bool _isVictory;
    private Texture2D _backgroundSnapShot;

    private Texture2D _pixel;
    private SpriteFont _font;

    private Rectangle _screenBounds;

    public GameOverScene(int score, bool isVictory, Texture2D background)
    {
        _finalScore = score;
        _isVictory = isVictory;
        _backgroundSnapShot = background;
    }

    public override void LoadContent()
    {
        _pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        _font = Content.Load<SpriteFont>("fonts/04B_30");
        _screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;
    }

    public override void Draw(GameTime gameTime)
    {
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        if (_backgroundSnapShot != null)
            Core.SpriteBatch.Draw(_backgroundSnapShot, Vector2.Zero, Color.White);
        else
            Core.SpriteBatch.Draw(_pixel, _screenBounds, Color.Black);
        Core.SpriteBatch.Draw(_pixel, _screenBounds, Color.Black * 0.6f);
        
        Core.SpriteBatch.End();
    }

    public override void UnloadContent()
    {
        _backgroundSnapShot?.Dispose();
        _pixel?.Dispose();
    }
}