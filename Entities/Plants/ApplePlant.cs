using System;
using Microsoft.Xna.Framework;
using Sprint2.Levels;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class ApplePlant(BaseLevel level, (int, int) root) : Plant(level, Species.apple, root)
{
    private readonly int maxCells = new Random().Next(10, 21);

    public override void Update(GameTime gameTime)
    {
        for (int i = 0; i < Ticker.TicksPassed(gameTime); i++)
        {
            Grow();
        }
    }

    protected override void Grow()
    {
        BlockList newGrowth = new();
        if (CellsGrown < maxCells) {
            foreach ((int x, int y) in BudCells.Positions())
            {
                foreach ((int dx, int dy) in Funcs.ListShuffle(PlantUtil.GrowDirs))
                {
                    if (TryGrow(newGrowth, (x + dx, y + dy))) break;
                }
            }
        }

        //Move buds to stem, and replenish new buds
        StemCells.Union(BudCells);
        BudCells.Clear();
        BudCells.Union(newGrowth);
    }
}