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
        HashSet<(int, int)> newGrowth = [];
        foreach ((int x, int y) in BudCells)
        {
            foreach ((int dx, int dy) in Funcs.ListShuffle(PlantUtil.growDirs))
            {
                (int, int) newCell = (x + dx, y + dy);
                if (!StemCells.Contains(newCell) && !BudCells.Contains(newCell))
                {
                    newGrowth.Add(newCell);
                    break;
                }
            }
        }

        //Move buds to stem, and replenish new buds
        StemCells.UnionWith(BudCells);
        BudCells = newGrowth; //this very possibly might not do what i want
    }
}