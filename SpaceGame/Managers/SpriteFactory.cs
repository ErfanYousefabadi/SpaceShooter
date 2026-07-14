using System.Collections.Generic;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Managers;

public class SpriteFactory
{
    private TextureAtlas _atlas;
    private TextureRegion _shipRegion;
    private TextureRegion _lightBlueBulletRegion;
    private TextureRegion _heavyBlueBulletRegion;
    private TextureRegion _redBulletRegion;
    private Animation _explosionAnimation;
    private Animation _goldCoinAnimation;
    private Animation _silverCoinAnimation;

    private Dictionary<EnemyType, TextureRegion> _enemyRegions;

    public SpriteFactory(TextureAtlas atlas)
    {
        _atlas = atlas;

        _shipRegion = _atlas.GetRegion("ship");
        _redBulletRegion = _atlas.GetRegion("red-bullet");
        _lightBlueBulletRegion = _atlas.GetRegion("light-blue-bullet");
        _heavyBlueBulletRegion = _atlas.GetRegion("heavy-blue-bullet");
        _explosionAnimation = _atlas.GetAnimation("explosion-animation");
        _goldCoinAnimation = _atlas.GetAnimation("gold-animation");
        _silverCoinAnimation = _atlas.GetAnimation("silver-animation");

        _enemyRegions = new Dictionary<EnemyType, TextureRegion>
        {
            { EnemyType.Standard, _atlas.GetRegion("standard-enemy") },
            { EnemyType.Scout, _atlas.GetRegion("scout-enemy") },
            { EnemyType.Shooter, _atlas.GetRegion("shooter-enemy") },
            { EnemyType.HeavyTank, _atlas.GetRegion("heavytank-enemy") },
            { EnemyType.Terrorist, _atlas.GetRegion("terrorist-enemy") },
        };
    }

    public Animation ExplosionAnimation => _explosionAnimation;
    public Animation GoldAnimation => _goldCoinAnimation;
    public Animation SilverAnimation => _silverCoinAnimation;
    public TextureRegion LightBlueBulletRegion => _lightBlueBulletRegion;
    public TextureRegion HeavyBlueBulletRegion => _heavyBlueBulletRegion;
    public TextureRegion RedBulletRegion => _redBulletRegion;

    public Sprite CreateShipSprite() 
        => CenterOrigin(new Sprite(_shipRegion));

    public Sprite CreateHeavyBlueBulletSprite() 
        => CenterOrigin(new Sprite(_heavyBlueBulletRegion) {Scale = new(0.5f, 0.5f)});

    public Sprite CreateLightBlueBulletSprite() 
        => CenterOrigin(new Sprite(_lightBlueBulletRegion));

    public Sprite CreateRedBulletSprite() 
        => CenterOrigin(new Sprite(_redBulletRegion) {Scale = new(1, 0.5f)});

    public Sprite CreateEnemySprite(EnemyType type)
        => CenterOrigin(new Sprite(_enemyRegions[type]));

    public AnimatedSprite CreateExplosionSprite() 
        => CenterOrigin(new AnimatedSprite(_explosionAnimation));
    
    public AnimatedSprite CreateGoldCoinSprite()
        => CenterOrigin(new AnimatedSprite(_goldCoinAnimation) {Scale = new(1.5f, 1.5f)});

    public AnimatedSprite CreateSilverCoinSprite()
        => CenterOrigin(new AnimatedSprite(_silverCoinAnimation) {Scale = new(1.5f, 1.5f)});

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