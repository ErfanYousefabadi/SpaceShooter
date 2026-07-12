using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;

namespace SpaceGame.Scenes;

public class GameOverScene : Scene
{
    private int _finalScore;
    private bool _isVictory;
    private Texture2D _backgroundSnapShot;

    private Texture2D _pixel;
    private SpriteFont _font, _fontBig;

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

        _font = Content.Load<SpriteFont>("fonts/04B_30_25");
        _fontBig = Content.Load<SpriteFont>("fonts/04B_30_87");
        _screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
            Core.ChangeScene(new GamePlayScene());
    }

    public override void Draw(GameTime gameTime)
    {
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        if (_backgroundSnapShot != null)
            Core.SpriteBatch.Draw(_backgroundSnapShot, Vector2.Zero, Color.White);
        else
            Core.SpriteBatch.Draw(_pixel, _screenBounds, Color.Black);
        Core.SpriteBatch.Draw(_pixel, _screenBounds, Color.Black * 0.7f);

        string title = _isVictory ? "VICTORY" : "GAME OVER";
        Vector2 titleSize = _fontBig.MeasureString(title);
        Vector2 titlePos = new((_screenBounds.Width - titleSize.X) * 0.5f, 100);
        Core.SpriteBatch.DrawString( _fontBig, title, titlePos + Vector2.One * 10, Color.Black);
        Core.SpriteBatch.DrawString( _fontBig, title, titlePos, _isVictory ? Color.Gold : Color.OrangeRed);

        string scoreText = $"Final Score: {_finalScore}";
        Vector2 scoreSize = _font.MeasureString(scoreText);
        Vector2 scorePos = new((_screenBounds.Width - scoreSize.X) * 0.5f, 300);
        Core.SpriteBatch.DrawString(_font, scoreText, scorePos + Vector2.One * 5, Color.Black);
        Core.SpriteBatch.DrawString(_font, scoreText, scorePos, Color.White);

        string retry = "Press Enter to Retry";
        string quit = "Press ESC to Quit";
        Vector2 retrySize = _font.MeasureString(retry);
        Vector2 quitSize = _font.MeasureString(quit);
        Vector2 retryPos = new(5, _screenBounds.Height - retrySize.Y - 5);
        Vector2 quitPos = new(_screenBounds.Width - quitSize.X - 5, _screenBounds.Height - quitSize.Y - 5);
        Core.SpriteBatch.DrawString(_font, retry, retryPos + Vector2.One * 5, Color.Black);
        Core.SpriteBatch.DrawString(_font, retry, retryPos, Color.Gray);
        Core.SpriteBatch.DrawString(_font, quit, quitPos + Vector2.One * 5, Color.Black);
        Core.SpriteBatch.DrawString(_font, quit, quitPos, Color.Gray);

        Core.SpriteBatch.End();
    }

    public override void UnloadContent()
    {
        _backgroundSnapShot?.Dispose();
        _pixel?.Dispose();
    }
}