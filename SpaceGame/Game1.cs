using Microsoft.Xna.Framework;
using MonoGameLibrary;
using SpaceGame.Scenes;

namespace SpaceGame;

public class Game1 : Core
{
    public Game1() : base("Space Game", 1280, 720, false)
    {

    }

    protected override void Initialize()
    {
        base.Initialize();

        ChangeScene(new GamePlayScene());
    }

    protected override void LoadContent()
    {
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}