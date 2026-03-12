using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities;
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
    //player
    protected Player Player {get;}
    //managers
    protected ProjectileManager ProjectileManager {get;}
    protected CollisionManager CollisionManager {get;} = new();
    public void AddBlockList(BlockList blocks) => CollisionManager.Blocks.Add(blocks); //bad for coupling if public
    public bool HasBlockAt((int, int) pos) => CollisionManager.HasBlockAt(pos); //bad for coupling if public
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

    public void TrySow(ProjectileType type, (int, int) coords) { //this is mildly silly
        var (x, y) = coords;
        int leftRightRand = Funcs.RandInt(0, 2)*2-1;
        foreach (var pos in new[]{(x, y-1), (x+leftRightRand, y), (x-leftRightRand, y), (x,y+1)}) {
            if (!CollisionManager.HasBlockAt(pos)) {
                Plants.Add(Projectile.ProjectileToPlant[type](this, coords));
                break;
            }
        }
    }

    public BlockType? TryDigBelow(Vector2 coords) {
        var (x, y) = Funcs.GridCoords(coords);
        y++; //want the cell *below* midpoint of bottom edge of player

        var output = CollisionManager.TryBreakBlockAt((x, y));
        if (output is null) {
            if (coords.X % Consts.BlockWidth < Consts.BlockWidth / 2f)
                output = CollisionManager.TryBreakBlockAt((x-1, y));
            else
                output = CollisionManager.TryBreakBlockAt((x+1, y));
        }
        return output;
    }

    public void Update(GameTime gameTime)
    {
        if (IsOver) return;

        //update entities
        ProjectileManager.Update(gameTime, CollisionManager);
        Player.Update(gameTime, CollisionManager);
        EnemyManager.Update(gameTime, Player, ProjectileManager, CollisionManager);

        //check for player digging logic
        if (Player.IsBreakable) {
            var type = TryDigBelow(new(Player.Collider.Center.X, Player.Collider.Bottom));
            if (type is not null) {
                if (Plant.BlockToSpecies.TryGetValue(type.Value, out Species value)) Player.GetSeed(value);
                Player.IsBreakable = false;
            }
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