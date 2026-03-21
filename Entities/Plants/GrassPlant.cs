using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class GrassPlant(BlockManager blockManager, (int, int) root) : Plant(blockManager, root, Species.Grass)
{
    private new readonly int MaxCells = Funcs.RandInt(5, 8);

    public override void Update(GameTime gameTime) {
        if (IsGrowing) for (int i = 0; i < Ticker.TicksPassed(gameTime); i++) Grow();
    }

    protected override void Grow() {
        HashSet<(int, int)> newGrowth = [];

        foreach ((int x, int y) in BudCells) {
            BlockManager.SetColorAt((x, y), Color.Gray);
            if (IsGrowing && CellsGrown < MaxCells) TryGrow(newGrowth, (x, y - 1));
            else IsGrowing = false;
        }

        //Move buds to stem, and replenish new buds
        StemCells.UnionWith(BudCells);
        BudCells.Clear();
        BudCells.UnionWith(newGrowth);
    }
}