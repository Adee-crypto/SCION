using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public enum Species {
    Grass, Apple, Pineapple, Sandbox, Void, Cherry
};

public abstract class Plant
{
    protected BlockManager BlockManager {get;}

    //init fields
    protected Species Species { get; }
    protected int MaxCells { get; }
    protected (int x, int y) Root { get; }

    //data & state
    protected List<(int x, int y)> BudCells { get; } = [];
    protected List<(int x, int y)> StemCells { get; } = [];
    protected int CellsGrown {get; set; }
    protected bool IsGrowing {get; set;} = true;
    protected Ticker Ticker { get; }

    /// <summary>root MUST be free in blockManager</summary>
    public Plant(BlockManager blockManager, (int, int) root, Species species)
    {
        BlockManager = blockManager;
        Species = species;
        Root = root;
        MaxCells = PlantUtil.SpeciesMaxCells(species);
        Ticker = new(PlantUtil.SpeciesGrowTimes[species]);
        BudCells.Add(root);
        blockManager.Set(root, PlantUtil.SpeciesToBlock[species]);
    }

    public abstract void Update(GameTime gameTime);

    //handles all growing logic
    protected abstract void Grow();

    protected void MatureAllBuds() {
        BudCells.ForEach(TryMatureCell);
        BudCells.Clear();
    }

    protected void TryMatureCell((int, int) pos) {
        if (BlockManager.HasBlockAt(pos)) {
            BlockManager.SetColor(pos, Color.Gray);
            StemCells.Add(pos);
        }
    }

    /// <summary> returns if it can grow into newCellPos, then grows there </summary>
    protected bool TryGrow((int, int) newCellPos) {
        if (IsGrowing && !BlockManager.HasBlockAt(newCellPos)) {
            Assets.PlantGrowthSFX(Species).Play(1, Funcs.SpeciesScale(Species), 0);
            BudCells.Add(newCellPos);
            BlockManager.Add(newCellPos, PlantUtil.SpeciesToBlock[Species], Color.White);
            CellsGrown++;
            if (CellsGrown >= MaxCells) {
                MatureAllBuds();
                IsGrowing = false;
            }
            return true;
        }
        return false;
    }
}