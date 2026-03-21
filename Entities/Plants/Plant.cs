using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Extensions;
using Sprint2.Managers;

namespace Sprint2.Entities.Plants;

public enum Species {
    Grass, Apple, Pineapple, Sandbox
};

public abstract class Plant
{
    //init fields
    protected Species Species { get; init; }
    protected (int x, int y) Root {get; init;}

    //cells
    protected HashSet<(int x, int y)> BudCells { get; set; } = [];
    protected HashSet<(int x, int y)> StemCells { get; } = [];
    protected int CellsGrown {get; set;}
    protected int MaxCells {get; set;} = int.MaxValue;

    protected bool IsGrowing {get; set;} = true;
    protected Ticker Ticker { get; }
    protected BlockManager BlockManager {get;}

    /// <summary>root MUST be free in blockManager</summary>
    public Plant(BlockManager blockManager, (int, int) root, Species species)
    {
        BlockManager = blockManager;
        Species = species;
        Root = root;
        Ticker = new(PlantUtil.SpeciesGrowTimes[species]);
        BudCells.Add(root);
        blockManager.Add(root, PlantUtil.SpeciesToBlock[species]);
    }

    public abstract void Update(GameTime gameTime);

    //handles all growing logic
    protected abstract void Grow();

    //returns if it can grow into newCellPos, then grows there
    protected bool TryGrow(HashSet<(int, int)> newGrowth, (int, int) newCellPos) {
        if (!BlockManager.HasBlockAt(newCellPos) && CellsGrown < MaxCells) {
            newGrowth.Add(newCellPos);
            BlockManager.Add(newCellPos, PlantUtil.SpeciesToBlock[Species], Color.White);
            CellsGrown++;
            return true;
        }
        return false;
    }
}