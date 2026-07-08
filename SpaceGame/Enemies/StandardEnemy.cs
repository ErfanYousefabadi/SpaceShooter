using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Enemies;

public class StandardEnemy : Enemy
{
    public StandardEnemy(Sprite sprite, Vector2 pos, float baseSpeed, int baseHP, 
        int scoreValue, float coinDropChance, Vector2? target) 
        : base(sprite, pos, baseSpeed, baseHP, scoreValue, coinDropChance, target) {}

    public StandardEnemy(AnimatedSprite sprite, Vector2 pos, float baseSpeed, int baseHP, 
        int scoreValue, float coinDropChance, Vector2? target) 
        : base(sprite, pos, baseSpeed, baseHP, scoreValue, coinDropChance, target) {}

    public override void Move(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;    

        var newPos = Position;
        newPos.Y += Speed * deltaTime;

        Position = newPos;
    }
}