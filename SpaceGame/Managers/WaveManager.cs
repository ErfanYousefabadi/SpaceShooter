using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SpaceGame.Enemies;
using MonoGameLibrary.Graphics;

namespace SpaceGame.Managers;

public enum EnemyType { Standard, Scout, Shooter, HeavyTank, Terrorist }

public class WaveConfig
{
    public int TotalEnemies;
    public Dictionary<EnemyType, int> Weights;
}

public class WaveManager
{
    private TimeSpan _spawnTimer = TimeSpan.Zero;
    private TimeSpan _interval;
    private Queue<EnemyType> _spawnQueue = [];
    private Random rng = new();
    private const int TOTAL_WAVES = 10;
    private const int LANES_NUMBER = 8;
    private const int WIDTH_OF_ALL_LANES = 1280;
    private static readonly Dictionary<int, WaveConfig> WaveTable = new();
    private int _previousLane = -1;
    private static readonly Dictionary<EnemyType, Sprite> Sprites = [];

    public int CurrentWave { get; private set; }
    public bool IsWaveComplete { get; private set; }

    static WaveManager()
    {
        WaveConfig w1 = new()
        {
            TotalEnemies = 6,
            Weights = new() { { EnemyType.Standard, 100 } }
        };
        WaveTable.Add(1, w1);
        
        WaveConfig w2 = new()
        {
            TotalEnemies = 8,
            Weights = new() { { EnemyType.Standard, 70 }, { EnemyType.Scout, 30 } }
        };
        WaveTable.Add(2, w2);

        WaveConfig w3 = new()
        {
            TotalEnemies = 10,
            Weights = new() { { EnemyType.Standard, 50 }, { EnemyType.Scout, 30 }, { EnemyType.Shooter, 20 } }
        };
        WaveTable.Add(3, w3);

        WaveConfig w4 = new()
        {
            TotalEnemies = 12,
            Weights = new() 
            { 
                { EnemyType.Standard, 35 }, 
                { EnemyType.Scout, 25 }, 
                { EnemyType.Shooter, 25 },
                { EnemyType.Terrorist, 15 }
            }
        };
        WaveTable.Add(4, w4);

        WaveConfig w5 = new()
        {
            TotalEnemies = 14,
            Weights = new() 
            { 
                { EnemyType.Standard, 25 }, 
                { EnemyType.Scout, 20 }, 
                { EnemyType.Shooter, 25 },
                { EnemyType.Terrorist, 20 },
                { EnemyType.HeavyTank , 10 }
            }
        };
        WaveTable.Add(5, w5);

        WaveConfig w6 = new()
        {
            TotalEnemies = 16,
            Weights = new() 
            { 
                { EnemyType.Standard, 20 }, 
                { EnemyType.Scout, 15 }, 
                { EnemyType.Shooter, 25 },
                { EnemyType.Terrorist, 20 },
                { EnemyType.HeavyTank , 20 }
            }
        };
        WaveTable.Add(6, w6);

        WaveConfig w7 = new()
        {
            TotalEnemies = 18,
            Weights = new() 
            { 
                { EnemyType.Standard, 15 }, 
                { EnemyType.Scout, 15 }, 
                { EnemyType.Shooter, 25 },
                { EnemyType.Terrorist, 20 },
                { EnemyType.HeavyTank , 25 }
            }
        };
        WaveTable.Add(7, w7);

        WaveConfig w8 = new()
        {
            TotalEnemies = 20,
            Weights = new() 
            { 
                { EnemyType.Standard, 10 }, 
                { EnemyType.Scout, 10 }, 
                { EnemyType.Shooter, 25 },
                { EnemyType.Terrorist, 25 },
                { EnemyType.HeavyTank , 30 }
            }
        };
        WaveTable.Add(8, w8);

        WaveConfig w9 = new()
        {
            TotalEnemies = 22,
            Weights = new() 
            { 
                { EnemyType.Standard, 10 }, 
                { EnemyType.Scout, 10 }, 
                { EnemyType.Shooter, 20 },
                { EnemyType.Terrorist, 25 },
                { EnemyType.HeavyTank , 35 }
            }
        };
        WaveTable.Add(9, w9);

        WaveConfig w10 = new()
        {
            TotalEnemies = 22,
            Weights = new() 
            { 
                { EnemyType.Standard, 5 }, 
                { EnemyType.Scout, 5 }, 
                { EnemyType.Shooter, 20 },
                { EnemyType.Terrorist, 25 },
                { EnemyType.HeavyTank , 45 }
            }
        };
        WaveTable.Add(10, w10);
    }

    public WaveManager(Sprite standard, Sprite scout, Sprite shooter, Sprite heavy, Sprite terrorist)
    {
        Sprites.Add(EnemyType.Standard, standard);
        Sprites.Add(EnemyType.Scout, scout);
        Sprites.Add(EnemyType.Shooter, shooter);
        Sprites.Add(EnemyType.Terrorist, terrorist);
        Sprites.Add(EnemyType.HeavyTank, heavy);
    }

    public void StartWave(int waveNumber)
    {
        if (waveNumber < 1 || waveNumber > TOTAL_WAVES)
            throw new InvalidOperationException($"Waves must be between 1 and {TOTAL_WAVES}");
        IsWaveComplete = false;
        _interval = TimeSpan.FromSeconds(2) - (waveNumber * TimeSpan.FromMilliseconds(120));
        _spawnTimer = TimeSpan.Zero;
        CurrentWave = waveNumber;
        _spawnQueue.Clear();
        int enemyNumber = WaveTable[CurrentWave].TotalEnemies;

        while (enemyNumber-- > 0)
        {
            var type = PickWeightedEnemyType(WaveTable[CurrentWave].Weights);
            _spawnQueue.Enqueue(type);
        }
    }

    public void Update(GameTime gameTime, List<Enemy> activeEnemies)
    {
        if (activeEnemies.Count == 0 && _spawnQueue.Count == 0)
        {
            WaveCompleted?.Invoke(CurrentWave);
            IsWaveComplete = true;
        }

        if (_spawnQueue.Count == 0) return;
        _spawnTimer += gameTime.ElapsedGameTime;

        if (_spawnTimer >= _interval)
        {
            _spawnTimer -= _interval;
            var type = _spawnQueue.Dequeue();
            activeEnemies.Add(CreateEnemy(type, Sprites[type]));
        }
    }

    private Enemy CreateEnemy(EnemyType type, Sprite enemySprite)
    {
        enemySprite.CenterOrigin();
        int lane, backupLane; // scout enemeis should not be in left most or right most lane
        int laneWidth = WIDTH_OF_ALL_LANES / LANES_NUMBER;

        do {
            lane = rng.Next(8);
            backupLane = rng.Next(1, 7);
        } while (lane == _previousLane && backupLane != _previousLane);

        _previousLane = lane;

        Vector2 pos = new((lane + 0.5f) * laneWidth, -enemySprite.Height * 0.5f);
        Vector2 backupPos = new((backupLane + 0.5f) * laneWidth, -enemySprite.Height * 0.5f);

        switch(type)
        {
            case EnemyType.Standard:
                var en1 = new StandardEnemy(enemySprite, pos);
                en1.ApplyWaveScaling(CurrentWave);
                return en1;
            case EnemyType.Scout:
                var en2 = new ScoutEnemy(enemySprite, backupPos);
                en2.ApplyWaveScaling(CurrentWave);
                return en2;
            case EnemyType.Shooter:
                var en3 = new ShooterEnemy(enemySprite, pos);
                en3.ApplyWaveScaling(CurrentWave);
                return en3;
            case EnemyType.HeavyTank:
                var en4 = new HeavyTankEnemy(enemySprite, pos);
                en4.ApplyWaveScaling(CurrentWave);
                return en4;
            case EnemyType.Terrorist:
                var en5 = new TerroristEnemy(enemySprite, pos, null);
                en5.ApplyWaveScaling(CurrentWave);
                return en5;
            default:
                return new StandardEnemy(enemySprite, pos);
        };
    }

    private EnemyType PickWeightedEnemyType(Dictionary<EnemyType, int> weights)
    {
        EnemyType answer = EnemyType.Standard;
        int sum = 0;
        int rand = rng.Next(100);
        foreach (var x in weights)
        {
            sum += x.Value;
            if (sum > rand)
            {
                answer = x.Key;
                break;
            }
        }
        return answer;
    }

    public event Action<int> WaveCompleted;
}