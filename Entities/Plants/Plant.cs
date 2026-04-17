using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public enum Species
{
    Grass,
    Apple,
    Pineapple,
    Sandbox,
    Void,
    Cherry,
    Gravebind,
    Catalyst
}

public abstract class Plant
{
    protected BlockManager BlockManager { get; }

    // Init fields
    protected Species Species { get; }
    protected int MaxCells { get; private set; }
    protected (int x, int y) Root { get; }

    // Data & state
    protected List<(int x, int y)> BudCells { get; } = [];
    protected List<(int x, int y)> StemCells { get; } = [];
    protected int CellsGrown { get; set; }
    protected bool IsGrowing { get; set; } = true;
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

    // Handles all growing logic
    protected abstract void Grow();

    protected void MatureAllBuds()
    {
        BudCells.ForEach(TryMatureCell);
        BudCells.Clear();
    }

    protected void TryMatureCell((int, int) pos)
    {
        if (BlockManager.HasBlockAt(pos))
        {
            BlockManager.SetColor(pos, Color.Gray);
            StemCells.Add(pos);
        }
    }

    /// <summary>
    /// Returns if it can grow into newCellPos, then grows there.
    /// Catalyst amplification increases the maximum growth space.
    /// </summary>
    protected bool TryGrow((int, int) newCellPos)
    {
        if (IsGrowing && !BlockManager.HasBlockAt(newCellPos))
        {
            // Catalyst increases growth space (more total cells before maturing)
            float amp = CatalystFlowerPlant.GetAmplificationFactor(BlockManager, newCellPos, "spread");

            Assets.PlantGrowthSFX(Species).Play(1, Funcs.SpeciesScale(Species), 0);
            BudCells.Add(newCellPos);
            BlockManager.Add(newCellPos, PlantUtil.SpeciesToBlock[Species], Color.White);
            CellsGrown++;

            if (CellsGrown >= MaxCells * amp)
            {
                MatureAllBuds();
                IsGrowing = false;
            }
            return true;
        }
        return false;
    }
}