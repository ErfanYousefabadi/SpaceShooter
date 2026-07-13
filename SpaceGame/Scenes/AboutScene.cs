using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;

namespace SpaceGame.Scenes;

public class AboutScene : Scene
{
    private const string STUDENT_NAME = "Erfan Yousefabadi";
    private const string STUDENT_ID = "404522133";
    private const string TOOLS_USED = "Monogame";

    private SpriteFont _font, _fontBig;
    private Texture2D _pixel;

    private Rectangle _screenBounds;

    public override void Initialize()
    {
        base.Initialize();

        Core.ExitOnEscape = false;

        _screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        _pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }
    
    public override void LoadContent()
    {
        _font = Content.Load<SpriteFont>("fonts/04B_30");
        _fontBig = Content.Load<SpriteFont>("fonts/04B_30_87");
    }

    public override void UnloadContent()
    {
        base.UnloadContent();
        _pixel?.Dispose();
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape)
            || Core.Input.Keyboard.WasKeyJustPressed(Keys.A))
            Core.ChangeScene(new MainMenuScene());
    }

    public override void Draw(GameTime gameTime)
    {
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Core.SpriteBatch.Draw(_pixel, _screenBounds, new Color(10, 10, 20));

        var panel = new Rectangle(_screenBounds.Width / 2 - 320, 160, 640, 380);
        Core.SpriteBatch.Draw(_pixel, panel, Color.Black * 0.5f);
        DrawBorder(panel, Color.White * 0.4f, 2);

        int lineY = panel.Y + 40;
        const int lineSpacing = 50;

        DrawCentered(_fontBig, "ABOUT", lineY, Color.Gold);
        lineY += lineSpacing + 100;

        DrawCentered(_font, $"Name: {STUDENT_NAME}", lineY, Color.White);
        lineY += lineSpacing;

        DrawCentered(_font, $"Student ID: {STUDENT_ID}", lineY, Color.White);
        lineY += lineSpacing;

        DrawCentered(_font, $"Built with: {TOOLS_USED}", lineY, Color.LightGray);

        DrawCentered(_font, "Press ESC to return to Main Menu", panel.Bottom - 35, Color.LightGray);

        Core.SpriteBatch.End();
    }

    private void DrawCentered(SpriteFont font, string text, int y, Color color)
    {
        Vector2 size = font.MeasureString(text);
        Core.SpriteBatch.DrawString(font, text, new Vector2(_screenBounds.Width / 2f - size.X / 2f, y), color);
    }

    private void DrawBorder(Rectangle bounds, Color color, int thickness)
    {
        Core.SpriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Y, bounds.Width, thickness), color);
        Core.SpriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Bottom - thickness, bounds.Width, thickness), color);
        Core.SpriteBatch.Draw(_pixel, new Rectangle(bounds.X, bounds.Y, thickness, bounds.Height), color);
        Core.SpriteBatch.Draw(_pixel, new Rectangle(bounds.Right - thickness, bounds.Y, thickness, bounds.Height), color);
    }
}