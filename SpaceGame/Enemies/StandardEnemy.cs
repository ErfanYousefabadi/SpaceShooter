using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Enemies;

public class StandardEnemy : Enemy
{
    private const int SPRITE_SIZE = 48;
    private const int BASE_HP = 20;
    private const int BASE_SPEED = 120;
    private const int CONTACT_DAMAGE = 10;
    private const int SCORE_VALUE = 50;
    private const float COIN_DROP = 0.4f;

    public StandardEnemy(Sprite sprite, Vector2 pos) 
        : base(sprite, pos, BASE_SPEED, BASE_HP, SCORE_VALUE, COIN_DROP, null, CONTACT_DAMAGE)
    {
        CoinDropType = Entities.CoinType.Silver;
        _sprite.Scale = Vector2.One * SPRITE_SIZE / _sprite.Region.Width;
    }

    public override void Move(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;    

        var newPos = Position;
        newPos.Y += Speed * deltaTime;

        Position = newPos;
    }
}