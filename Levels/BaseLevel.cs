using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Players;
using Sprint2.Extensions;
using Sprint2.Managers;
using System.Collections.Generic;

namespace Sprint2.Levels;

public abstract class BaseLevel(Player player) : ILevel
{
    //player
    protected Player Player {get;} = player;
    //managers
    protected ProjectileManager ProjectileManager {get;} = new(player);
    protected CollisionManager CollisionManager {get;} = new();
    protected EnemyManager EnemyManager {get;} = new();
    protected HUDManager HudManager {get;} = new(player);
    //blocks
    protected List<Platform> Platforms { get; } = [];
    protected List<Plant> Plants { get; } = [];
    //state variables
    protected (int w, int h) ScreenSize { get; private set; }
    public bool IsOver { get; protected set; }
    public LevelEndReason EndReason { get; protected set; } = LevelEndReason.None;

    public void Resize((int w, int h) size)
    {
        ScreenSize = size;
        HudManager.Resize(size);
    }

    public virtual void Reset()
    {
        IsOver = false;
        EndReason = LevelEndReason.None;

        player.Reset();
        ProjectileManager.Reset();
        CollisionManager.Reset();
        EnemyManager.Reset();

        Platforms.Clear();
        Plants.Clear();

        BuildLevel();
    }

    protected abstract void BuildLevel();

    protected virtual void UpdateLevelLogic(GameTime gameTime) { }

    public void Update(GameTime gameTime)
    {
        if (IsOver) return;

        //Update blocks to collide with
        CollisionManager.Reset();
        Plants.ForEach(p => CollisionManager.Blocks.Union(p.Blocks));
        Platforms.ForEach(p => CollisionManager.Blocks.Union(p.Blocks));

        //update entities
        ProjectileManager.Update(gameTime, CollisionManager);
        player.Update(gameTime, CollisionManager);
        EnemyManager.Update(gameTime, player, ProjectileManager, CollisionManager);

        //check for player digging logic
        if (player.IsBreakable)
        {
            foreach (var p in Plants)
            {
                if (p.TryRemoveCellBelow(new Vector2(player.Collider.Center.X, player.Collider.Bottom)))
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
        Platforms.ForEach(p => p.Draw(spriteBatch));
        Plants.ForEach(p => p.Draw(spriteBatch));

        EnemyManager.Draw(spriteBatch);
        ProjectileManager.Draw(spriteBatch);
        HudManager.Draw(spriteBatch);

        player.Draw(spriteBatch);
    }
}