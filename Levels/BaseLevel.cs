using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities;
using Sprint2.Entities.Enemies;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Players;
using Sprint2.Extensions;
using Sprint2.Managers;
using System.Collections.Generic;
using System.Linq;

namespace Sprint2.Levels;

public abstract class BaseLevel(Player player) : ILevel
{
    private readonly Player player = player;
    private readonly ProjectileManager projectileManager = new(player);
    private readonly CollisionManager collisionManager = new();
    private readonly EnemyManager enemyManager = new();
    private readonly HUDManager hudManager = new(player);

    protected Player Player => player;
    protected ProjectileManager ProjectileManager => projectileManager;
    protected CollisionManager CollisionManager => collisionManager;
    protected EnemyManager EnemyManager => enemyManager;
    protected HUDManager HudManager => hudManager;

    protected List<Platform> Platforms { get; } = [];
    protected List<Plant> Plants { get; } = [];
    protected BlockList Blocks { get; } = new();

    protected (int w, int h) ScreenSize { get; private set; }
    public bool IsOver { get; protected set; }
    public LevelEndReason EndReason { get; protected set; } = LevelEndReason.None;

    public void Resize((int w, int h) size)
    {
        ScreenSize = size;
        hudManager.Resize(size);
    }

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

    protected virtual void UpdateLevelLogic(GameTime gameTime) { }

    protected void SpawnEnemy(EnemyDef def, Vector2 pos)
    {
        enemyManager.Spawn(def, pos);
    }

    public void Update(GameTime gameTime)
    {
        if (IsOver) return;

        collisionManager.Reset();

        Plants.ForEach(p => collisionManager.Objects.AddRange(p.GetPlantObjects()));

        collisionManager.Objects.AddRange(Platforms.Select(p => p.PixelBounds));

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

        Plants.ForEach(p => p.Update(gameTime));

        UpdateLevelLogic(gameTime);

        if ((!IsOver && ScreenSize.h > 0 && player.Collider.Position.Y > ScreenSize.h) || player.IsDead)
        {
            IsOver = true;
            EndReason = LevelEndReason.PlayerDied;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Plants.ForEach(p => p.Draw(spriteBatch));

        projectileManager.Draw(spriteBatch);
        enemyManager.Draw(spriteBatch);

        Platforms.ForEach(p => p.Draw(spriteBatch));

        hudManager.Draw(spriteBatch);

        player.Draw(spriteBatch);
    }
}