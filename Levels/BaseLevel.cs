using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Players;
using Sprint2.Entities.Projectiles;
using Sprint2.Extensions;
using Sprint2.Managers;
using System.Collections.Generic;

namespace Sprint2.Levels;

public abstract class BaseLevel : ILevel
{
    //player
    protected Player Player {get;}
    //managers
    protected ProjectileManager ProjectileManager {get;}
    protected CollisionManager CollisionManager {get;} = new();
    protected EnemyManager EnemyManager {get;} = new();
    protected HUDManager HudManager {get;}
    //blocks
    protected List<Platform> Platforms { get; } = [];
    protected List<Plant> Plants { get; } = [];
    //state variables
    protected (int w, int h) ScreenSize { get; private set; }
    public bool IsOver { get; protected set; }
    public LevelEndReason EndReason { get; protected set; } = LevelEndReason.None;

    public BaseLevel(Player player) {
        Player = player;
        ProjectileManager = new(this, player);
        HudManager = new(player);
    }

    public void Resize((int w, int h) size)
    {
        ScreenSize = size;
        HudManager.Resize(size);
    }

    public virtual void Reset()
    {
        IsOver = false;
        EndReason = LevelEndReason.None;

        Player.Reset();
        ProjectileManager.Reset();
        CollisionManager.Reset();
        EnemyManager.Reset();

        Platforms.Clear();
        Plants.Clear();

        BuildLevel();
    }

    protected abstract void BuildLevel();

    protected virtual void UpdateLevelLogic(GameTime gameTime) { }

    public void TryGrow(ProjectileType type, (int, int) coords) {
        if (!CollisionManager.Blocks.Contains(coords)) {
            Plants.Add(Projectile.ProjectileToPlant[type](CollisionManager, coords));
        }
    }

    public void Update(GameTime gameTime)
    {
        if (IsOver) return;

        //Update blocks to collide with
        CollisionManager.Reset();
        Plants.ForEach(p => CollisionManager.Blocks.Union(p.Blocks));
        Platforms.ForEach(p => CollisionManager.Blocks.Union(p.Blocks));

        //update entities
        ProjectileManager.Update(gameTime, CollisionManager);
        Player.Update(gameTime, CollisionManager);
        EnemyManager.Update(gameTime, Player, ProjectileManager, CollisionManager);

        //check for player digging logic
        if (Player.IsBreakable)
        {
            foreach (var p in Plants)
            {
                if (p.TryRemoveCellBelow(new Vector2(Player.Collider.Center.X, Player.Collider.Bottom)))
                {
                    Player.GetSeed();
                    break;
                }
            }

            Player.IsBreakable = false;
        }

        Plants.ForEach(p => p.Update(gameTime));

        UpdateLevelLogic(gameTime);

        if ((!IsOver && ScreenSize.h > 0 && Player.Collider.Position.Y > ScreenSize.h) || Player.IsDead)
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

        Player.Draw(spriteBatch);
    }
}