using Microsoft.Xna.Framework;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Graphics;
using MonoGameLibrary;
using SpaceGame.Managers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpaceGame.Scenes;

public class GamePlayScene : Scene
{
    private SpriteFont _font, _fontBig;
    private Texture2D _background;
    private TextureAtlas _atlas;
    private GameManager _gameManager;
    private SpriteFactory _spriteFactory;
    private SoundEffect _collect, _shoot, _explode;
    private SoundEffect _shooterShoot, _heavyShoot, _collide, _bulletHit;

    public override void Initialize()
    {
        base.Initialize();

        Core.ExitOnEscape = false;

        _spriteFactory = new(_atlas);

        _gameManager = new(
            Core.GraphicsDevice.PresentationParameters.Bounds,
            _spriteFactory,
            _background,
            _font, _fontBig,
            _collect, _explode, _shoot, _shooterShoot, 
            _heavyShoot, _collide, _bulletHit
        );
    }

    public override void LoadContent()
    {
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
        _font = Content.Load<SpriteFont>("fonts/04B_30");
        _fontBig = Content.Load<SpriteFont>("fonts/04B_30_87");
        _background = Content.Load<Texture2D>("images/background");
        _collect = Content.Load<SoundEffect>("audios/coin-collect");
        _explode = Content.Load<SoundEffect>("audios/explode");
        _shoot = Content.Load<SoundEffect>("audios/shoot");
        _shooterShoot = Content.Load<SoundEffect>("audios/shooter");
        _heavyShoot = Content.Load<SoundEffect>("audios/tank-shoot");
        _collide = Content.Load<SoundEffect>("audios/enemy-collide");
        _bulletHit = Content.Load<SoundEffect>("audios/metal-hit");
    }

    public override void Update(GameTime gameTime)
    {
        _gameManager.Update(gameTime);
        if (_gameManager.IsGameOver)
            Core.ChangeScene(
                new GameOverScene(
                    _gameManager.Score, 
                    _gameManager.IsVictory, 
                    _gameManager.EndingScreenshot
                )
            );
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        _gameManager.Draw();

        base.Draw(gameTime);
    }
}