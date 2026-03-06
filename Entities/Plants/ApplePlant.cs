using Microsoft.Xna.Framework;
using Sprint2.Util;
using System.Collections.Generic;

namespace Sprint2.Entities.Plants;

public class ApplePlant((int, int) root) : Plant(Species.apple, root)
{
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
        foreach ((int x, int y) in BudCells.Positions())
        {
            foreach ((int dx, int dy) in Funcs.ListShuffle(PlantUtil.GrowDirs))
            {
                var newCellPos = (x + dx, y + dy);
                if (!StemCells.Contains(newCellPos) && !BudCells.Contains(newCellPos))
                {
                    newGrowth.Add(newCellPos, SpeciesToBlockType(Species));
                    break;
                }
            }
        }

        //Move buds to stem, and replenish new buds
        StemCells.Union(BudCells);
        BudCells = newGrowth;
    }
}