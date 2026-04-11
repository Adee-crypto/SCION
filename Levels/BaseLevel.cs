using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Items;
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
    public Sword Sword { get; set; }
    //managers TODO: can these be made protected again in a way that works with composed classes (projectile, plants, etc.)?
    public ProjectileManager ProjectileManager {get;}
    // public void AddBlockList(BlockList blocks) => CollisionManager.Blocks.Add(blocks); //bad for coupling if public
    // public bool HasBlockAt((int, int) pos) => CollisionManager.HasBlockAt(pos); //bad for coupling if public
    public EnemyManager EnemyManager {get;}
    public HUDManager HudManager {get;}
    public BlockManager BlockManager {get;} = new();
    public CollisionManager CollisionManager {get;}

    //static level elements (all with blocks for now)
    protected List<Plant> Plants { get; } = [];

    //state variables
    protected (int w, int h) ScreenSize { get; private set; }
    public bool IsOver { get; protected set; }
    public LevelEndReason EndReason { get; protected set; } = LevelEndReason.None;

    public BaseLevel(Player player) {
        Player = player;
        Sword = new(player);
        ProjectileManager = new(this, player);
        EnemyManager = new(this);
        HudManager = new(player);
        CollisionManager = new(BlockManager);
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
        Sword.Reset();
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

    public void TryPlantCherry()
    {
        if (Player.IsDead || IsOver) return;

        // Root position = grid coords under the player's feet (bottom center)
        var root = Funcs.GridCoords(new Vector2(Player.Collider.Center.X, Player.Collider.Bottom));

        Plants.Add(new CherryPlant(BlockManager, root));

        // Force collision resolution so the player is pushed on top of the new structure
        CollisionManager.ManageBlockCollision(Player.Collider, Vector2.Zero);
    }

    public void Infect((int, int) pos) => Plants.Add(new VoidPlant(BlockManager, pos));

    public void Update(GameTime gameTime)
    {
        if (IsOver) return;

        //update entities
        ProjectileManager.Update(gameTime);
        Player.Update(gameTime, CollisionManager);
        Sword.Update(gameTime, Player);
        EnemyManager.Update(gameTime);

        //check for player digging logic
        if (Player.IsBreakable) {
            if (BlockManager.TryDigBelow(new(Player.Collider.Center.X, Player.Collider.Bottom)) is BlockManager.Block block
                && PlantUtil.BlockToSpecies.TryGetValue(block.Type, out Species value))
                    Player.GetSeed(value);
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
        EnemyManager.Draw(spriteBatch);
        ProjectileManager.Draw(spriteBatch);
        HudManager.Draw(spriteBatch);
        BlockManager.Draw(spriteBatch);
        Player.Draw(spriteBatch);
        Sword.Draw(spriteBatch);
    }
}