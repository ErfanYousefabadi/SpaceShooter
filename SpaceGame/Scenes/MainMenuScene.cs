using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Scenes;

namespace SpaceGame.Scenes;

public class MainMenuScene : Scene
{
    private Texture2D _pixel;
    private SpriteFont _font, _fontBig;
    private Rectangle _screenBounds;
    private const string SPACE_TEXT = "Space";
    private const string SHOOTER_TEXT = "Shooter";
    private const string PRESS_ENTER_TEXT = "Press Enter to Start";

    private Vector2 _spacePos, _spaceOrigin;
    private Vector2 _shooterPos, _shooterOrigin;
    private Vector2 _enterPos, _enterOrigin;

    public override void Initialize()
    {
        base.Initialize();

        Core.ExitOnEscape = true;

        _screenBounds = Core.GraphicsDevice.PresentationParameters.Bounds;

        Vector2 size = _fontBig.MeasureString(SPACE_TEXT);
        _spacePos = new(640, 100);
        _spaceOrigin = size * 0.5f;

        size = _fontBig.MeasureString(SHOOTER_TEXT);
        _shooterPos = new(757, 207);
        _shooterOrigin = size * 0.5f;

        size = _font.MeasureString(PRESS_ENTER_TEXT);
        _enterPos = new(640, 620);
        _enterOrigin = size * 0.5f;
    }

    public override void LoadContent()
    {
        base.LoadContent();

        _pixel = new Texture2D(Core.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

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
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
            Core.ChangeScene(new GamePlayScene());
        else if (Core.Input.Keyboard.WasKeyJustPressed(Keys.A))
            Core.ChangeScene(new AboutScene());
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(10, 10, 20));

        Color shadow = Color.DarkGray * 0.5f;

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        Core.SpriteBatch.DrawString(
            _fontBig, SPACE_TEXT, 
            _spacePos + Vector2.One * 10, 
            shadow, 0, _spaceOrigin, 1, SpriteEffects.None, 0
        );
        Core.SpriteBatch.DrawString(
            _fontBig, SPACE_TEXT,
            _spacePos, Color.White,
            0, _spaceOrigin, 1, SpriteEffects.None, 0
        );

        Core.SpriteBatch.DrawString(
            _fontBig, SHOOTER_TEXT, 
            _shooterPos + Vector2.One * 10, 
            shadow, 0, _shooterOrigin, 1, SpriteEffects.None, 0
        );
        Core.SpriteBatch.DrawString(
            _fontBig, SHOOTER_TEXT,
            _shooterPos, Color.White,
            0, _shooterOrigin, 1, SpriteEffects.None, 0
        );

        Core.SpriteBatch.DrawString(
            _font, PRESS_ENTER_TEXT, _enterPos, 
            Color.White, 0, _enterOrigin
            , 1, SpriteEffects.None, 0
        );

        Core.SpriteBatch.End();
    }
}