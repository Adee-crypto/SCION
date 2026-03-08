using Microsoft.Xna.Framework;
using Sprint2.Entities;
using Sprint2.Entities.Enemies;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Players;
using Sprint2.Util;
using System;

namespace Sprint2.Levels;

public sealed class ArcadeLevel : BaseLevel
{
    private readonly EnemyDef rangedEnemy;

    private double elapsedTime;
    private double nextSpawnTime;
    private int wave;
    private float height;

    public ArcadeLevel(Player player) : base(player)
    {
        rangedEnemy = new EnemyDef("Void Spawn", Assets.VoidspawnTexture, 100, 98, 96, 128, 96);

        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        elapsedTime = 0;
        nextSpawnTime = 2;
        wave = 0;
        height = 0;
    }

    protected override void BuildLevel()
    {
        Platforms.Add(new Platform(BlockType.StoneBrick, 0, 25, 50, 1));
        Plants.Add(new ApplePlant(CollisionManager, (20, 20)));
        EnemyManager.Spawn(rangedEnemy, Consts.BlockWidth * new Vector2(40, 24));
    }

    protected override void UpdateLevelLogic(GameTime gameTime)
    {
        elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

        wave = (int)(elapsedTime / 10);

        if (elapsedTime >= nextSpawnTime)
        {
            int count = 1 + wave / 2;
            for (int i = 0; i < count; i++)
            {
                float spawnX = 30 + i * 2;
                float spawnY = 24;
                EnemyManager.Spawn(rangedEnemy, new Vector2(spawnX, spawnY));
            }

            double interval = Math.Max(0.75, 2 - wave * 0.2);
            nextSpawnTime = elapsedTime + interval;
        }

        height = Player.Collider.Position.Y;
    }
}