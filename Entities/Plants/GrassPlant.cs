using System;
using Microsoft.Xna.Framework;
using Sprint2.Levels;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class GrassPlant(BaseLevel level, (int, int) root) : Plant(level, Species.grass, root)
{
    private readonly int maxCells = new Random().Next(5, 8);

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
                TryGrow(newGrowth, (x, y - 1));
            }
        }

        //Move buds to stem, and replenish new buds
        StemCells.Union(BudCells);
        BudCells = newGrowth;
    }
}