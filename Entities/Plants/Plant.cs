using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Projectiles;
using Sprint2.Extensions;
using Sprint2.Levels;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public enum Species
{
    grass, apple, pineapple
};

public abstract class Plant
{
    public static Dictionary<Species, BlockType> SpeciesToBlock {get;} = new() {
        {Species.grass, BlockType.Grass},
        {Species.apple, BlockType.Apple},
        {Species.pineapple, BlockType.Pineapple},
    };

    public static Dictionary<Species, ProjectileType> SpeciesToProjectile {get;} = new() {
        {Species.grass, ProjectileType.Grass},
        {Species.apple, ProjectileType.Apple},
        {Species.pineapple, ProjectileType.Pineapple},
    };

    protected Species Species { get; }
    protected BlockList BudCells { get; set; } = new();
    protected BlockList StemCells { get; } = new();
    // public BlockList Blocks => new BlockList().Union(BudCells).Union(StemCells);
    protected float Age { get; set; }
    protected int CellsGrown {get; set;}
    protected Ticker Ticker { get; }
    private readonly BaseLevel level; //terrible for coupling, fix somehow

    public Plant(BaseLevel level, Species species, (int, int) root)
    {
        this.level = level;
        Ticker = new(PlantUtil.SpeciesGrowTimes[species]);
        Species = species;
        BudCells.Add(root, SpeciesToBlock[species]);
        level.AddBlockList(BudCells);
        level.AddBlockList(StemCells);
    }

    public abstract void Update(GameTime gameTime);

    //handles all growing logic
    protected abstract void Grow();

    //returns if it can grow into newCellPos, then grows there
    protected bool TryGrow(BlockList newGrowth, (int, int) newCellPos) {
        if (!level.HasBlockAt(newCellPos)) {
            newGrowth.Add(newCellPos, SpeciesToBlock[Species]);
            CellsGrown++;
            return true;
        }
        return false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        StemCells.Draw(spriteBatch, Color.Gray);
        BudCells.Draw(spriteBatch);
    }

    //FIX THIS TO ONLY BREAK IF PLAYER ACTUALLY TOUCHING BLOCK; work with collision?
    public bool TryRemoveCellBelow(Vector2 bottomCenter)
    {
        (int cellX, int cellY) pos = Funcs.GridCoords(bottomCenter);
        pos.cellY ++;
        if (StemCells.Contains(pos))
        {
            StemCells.Remove(pos);
            return true;
        }
        else if (StemCells.Contains((pos.cellX - 1, pos.cellY)) && bottomCenter.X % Consts.BlockWidth < Consts.BlockWidth / 2f)
        {
            StemCells.Remove((pos.cellX - 1, pos.cellY));
            return true;
        }
        else if (StemCells.Contains((pos.cellX + 1, pos.cellY)) && bottomCenter.X % Consts.BlockWidth > Consts.BlockWidth / 2f)
        {
            StemCells.Remove((pos.cellX + 1, pos.cellY));
            return true;
        }
        return false;
    }
}