using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Sprint2.Entities;
using Sprint2.Entities.Players;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.Entities.Plants;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Enemies;

namespace Sprint2.Levels;

public abstract class BaseLevel : ILevel
{
    private readonly Player player;
    private readonly ProjectileManager projectileManager;
    private readonly CollisionManager collisionManager;
    private readonly EnemyManager enemyManager;

    protected Player Player => player;
    protected ProjectileManager ProjectileManager => projectileManager;
    protected CollisionManager CollisionManager => collisionManager;
    protected EnemyManager EnemyManager => enemyManager;

    protected List<Platform> Platforms { get; }= [];
    protected List<Plant> Plants { get; }= [];

    protected (int w, int h) ScreenSize { get; private set; }
    public bool IsOver { get; protected set; }
    public LevelEndReason EndReason { get; protected set; } = LevelEndReason.None;

    protected BaseLevel(Player player)
    {
        this.player = player;
        projectileManager = new ProjectileManager(player);
        collisionManager = new CollisionManager();
        enemyManager = new EnemyManager();
    }

    public void Resize((int w, int h) size) => ScreenSize = size;

    public virtual void Reset()
    {
        IsOver = false;
        EndReason = LevelEndReason.None;

        player.Reset();
        projectileManager.Reset();
        collisionManager.Reset();
        enemyManager.Reset();

        Platforms.Clear();
        Plants.Clear();

        BuildLevel();
    }

    protected abstract void BuildLevel();

    protected virtual void UpdateLevelLogic(GameTime gameTime) {}

    protected void SpawnEnemy(EnemyDef def, Vector2 pos)
    {
        enemyManager.Spawn(def, pos);
    }

    public void Update(GameTime gameTime)
    {
        if (IsOver) return;

        collisionManager.Reset();

        foreach (var p in Plants) collisionManager.Objects.AddRange(p.GetPlantObjects());

        collisionManager.Objects.AddRange(Platforms.Select(p => p.Bounds));

        projectileManager.Update(gameTime, collisionManager);
        player.Update(gameTime, collisionManager);
        enemyManager.Update(gameTime, player, projectileManager, collisionManager);

        if (player.IsBreakable)
        {
            foreach (var p in Plants)
            {
                if (p.TryRemoveCellBelow(new Vector2(player.Collider.Hitbox.Center.X, player.Collider.Hitbox.Bottom)))
                {
                    player.GetSeed();
                    break;
                }
            }

            player.IsBreakable = false;
        }

        foreach (var p in Plants) p.Update(gameTime);

        UpdateLevelLogic(gameTime);

        if (!IsOver && ScreenSize.h > 0 && player.Collider.Position.Y > ScreenSize.h)
        {
            IsOver = true;
            EndReason = LevelEndReason.PlayerDied;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var p in Plants) p.Draw(spriteBatch);

        projectileManager.Draw(spriteBatch);
        enemyManager.Draw(spriteBatch);

        foreach (var p in Platforms) p.Draw(spriteBatch);

        player.Draw(spriteBatch);
    }
}