using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Levels;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class GrassPlant(BlockManager blockManager, (int, int) root) : Plant(blockManager, root, Species.Grass)
{
    private new readonly int MaxCells = Funcs.RandInt(5, 8);

    public override void Update(GameTime gameTime) {
        for (int i = 0; i < Ticker.TicksPassed(gameTime); i++) Grow();
    }

    protected override void Grow() {
        HashSet<(int, int)> newGrowth = [];
        if (CellsGrown < MaxCells) {
            foreach ((int x, int y) in BudCells) {
                TryGrow(newGrowth, (x, y - 1));
            }
        }

        //Move buds to stem, and replenish new buds
        StemCells.UnionWith(BudCells);
        BudCells.Clear();
        BudCells.UnionWith(newGrowth);
    }
}