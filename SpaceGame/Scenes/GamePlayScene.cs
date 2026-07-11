using Microsoft.Xna.Framework;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Graphics;
using MonoGameLibrary;
using SpaceGame.Managers;

namespace SpaceGame.Scenes;

public class GamePlayScene : Scene
{
    private TextureAtlas _atlas;
    private GameManager _gameManager;
    private SpriteFactory _spriteFactory;

    public override void Initialize()
    {
        base.Initialize();

        _spriteFactory = new(_atlas);

        _gameManager = new(Core.GraphicsDevice.PresentationParameters.Bounds, _spriteFactory);
    }

    public override void LoadContent()
    {
        _atlas = TextureAtlas.FromFile(Content, "images/atlas-definition.xml");
    }

    public override void Update(GameTime gameTime)
    {
        _gameManager.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.CornflowerBlue);

        _gameManager.Draw();

        base.Draw(gameTime);
    }
}