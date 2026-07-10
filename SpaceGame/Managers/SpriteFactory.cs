using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Managers;

public class SpriteFactory
{
    private TextureAtlas _atlas;
    private TextureRegion _shipRegion;
    private TextureRegion _bulletRegion;
    private Animation _explosionAnimation;

    private Dictionary<EnemyType, TextureRegion> _enemyRegions;

    public SpriteFactory(TextureAtlas atlas)
    {
        _atlas = atlas;

        _shipRegion = _atlas.GetRegion("ship");
        _bulletRegion = _atlas.GetRegion("bullet");
        _explosionAnimation = _atlas.GetAnimation("explosion-animation");

        _enemyRegions = new Dictionary<EnemyType, TextureRegion>
        {
            { EnemyType.Standard, _atlas.GetRegion("ship") },
            { EnemyType.Scout, _atlas.GetRegion("ship") },
            { EnemyType.Shooter, _atlas.GetRegion("ship") },
            { EnemyType.HeavyTank, _atlas.GetRegion("ship") },
            { EnemyType.Terrorist, _atlas.GetRegion("ship") },
        };
    }

    public Animation ExplosionAnimation => _explosionAnimation;

    public Sprite CreateShipSprite() 
        => CenterOrigin(new Sprite(_shipRegion));

    public Sprite CreateBulletSprite() 
        => CenterOrigin(new Sprite(_bulletRegion));

    public Sprite CreateEnemySprite(EnemyType type)
    {
        var ans = CenterOrigin(new Sprite(_enemyRegions[type]));
        ans.Rotation = MathHelper.Pi;
        return ans;
    }

    public AnimatedSprite CreateExplosionSprite() 
        => CenterOrigin(new AnimatedSprite(_explosionAnimation));
    
    private static Sprite CenterOrigin(Sprite x)
    {
        x.CenterOrigin();
        return x;
    }

    private static AnimatedSprite CenterOrigin(AnimatedSprite x)
    {
        x.CenterOrigin();
        return x;
    }
}