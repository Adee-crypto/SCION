using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Players;
using Sprint2.Entities.Projectiles;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.Util;
using System.Collections.Generic;

namespace Sprint2.Levels;

public abstract class BaseLevel : ILevel
{
    public Player Player {get;}
    //managers TODO: can these be made protected again in a way that works with composed classes (projectile, plants, etc.)?
    public ProjectileManager ProjectileManager { get; }
    // public void AddBlockList(BlockList blocks) => CollisionManager.Blocks.Add(blocks); //bad for coupling if public
    // public bool HasBlockAt((int, int) pos) => CollisionManager.HasBlockAt(pos); //bad for coupling if public
    public EnemyManager EnemyManager { get; }
    public HUDManager HudManager { get; }
    public BlockManager BlockManager { get; } = new();
    public CollisionManager CollisionManager { get; }

    //static level elements (all with blocks for now)
    protected List<Plant> Plants { get; } = [];

    //state variables
    protected (int w, int h) ScreenSize { get; private set; }
    public bool IsOver { get; protected set; }
    public LevelEndReason EndReason { get; protected set; } = LevelEndReason.None;

    public BaseLevel(Player player)
    {
        Player = player;
        CollisionManager = new(BlockManager);
        EnemyManager = new(this);
        ProjectileManager = new(
            EnemyManager,
            player,
            () => Controllers.MouseController.IsLeftClick(),
            (type, lifetime, pos, vel) => new Projectile(
                CollisionManager, type, lifetime, pos, vel,
                TrySow,
                coords => Infect(coords)));
        HudManager = new(player);
    }

    public void Resize(Vector2 size)
    {
        HudManager.Resize(size);
    }

    public virtual void Reset()
    {
        IsOver = false;
        EndReason = LevelEndReason.None;

        Player.Reset();
        ProjectileManager.Reset();
        EnemyManager.Reset();
        BlockManager.Reset();

        Plants.Clear();

        BuildLevel();
    }

    protected abstract void BuildLevel();

    protected virtual void UpdateLevelLogic(GameTime gameTime) { }

    public void TrySow(ProjectileType type, (int, int) prevCoords)
    {
        if (!BlockManager.HasBlockAt(prevCoords))
            Plants.Add(ProjectileUtil.ProjectileToPlant[type](BlockManager, prevCoords));
    }

    public void Infect((int, int) pos) => Plants.Add(new VoidPlant(BlockManager, pos));

    public void Update(GameTime gameTime)
    {
        if (IsOver) return;

        if (MediaPlayer.State != MediaState.Playing && MediaPlayer.State != MediaState.Paused) MediaPlayer.Play(Assets.BackgroundMusic);

        //update entities
        ProjectileManager.Update(gameTime);
        Player.Update(gameTime, CollisionManager);
        EnemyManager.Update(gameTime, CollisionManager);

        //check for player digging logic
        if (Player.IsBreakable)
        {
            if (BlockManager.TryDigBelow(new(Player.Collider.Center.X, Player.Collider.Bottom)) is BlockManager.Block block) {
                if (PlantUtil.BlockToSpecies.TryGetValue(block.Type, out Species value)) {
                    Player.GetSeed(value);
                } else if (block.Type == BlockManager.Block.BlockType.Warp) {
                    IsOver = true;
                    EndReason = LevelEndReason.Completed;
                }
            }
            Player.IsBreakable = false;
        }

        Plants.ForEach(p => p.Update(gameTime));

        UpdateLevelLogic(gameTime);

        if (Player.IsDead)
        {
            IsOver = true;
            MediaPlayer.Stop();
            EndReason = LevelEndReason.PlayerDied;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        EnemyManager.Draw(spriteBatch);
        ProjectileManager.Draw(spriteBatch);
        HudManager.Draw(spriteBatch);
        BlockManager.Draw(spriteBatch);
        Player.Draw(spriteBatch);
    }
}