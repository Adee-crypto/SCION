using Microsoft.Xna.Framework;
using Sprint2.Util;
using System.Collections.Generic;

namespace Sprint2.Entities.Plants;

public class ApplePlant((int, int) root) : Plant(Species.apple, root)
{
    public override void Update(GameTime gameTime)
    {
        for (int i = 0; i < ticker.TicksPassed(gameTime); i++)
        {
            Grow();
        }
    }

    protected override void Grow()
    {
        HashSet<(int, int)> newGrowth = [];
        foreach ((int x, int y) in budCells)
        {
            foreach ((int dx, int dy) in Funcs.ListShuffle(PlantUtil.growDirs))
            {
                (int, int) newCell = (x + dx, y + dy);
                if (!stemCells.Contains(newCell) && !budCells.Contains(newCell))
                {
                    newGrowth.Add(newCell);
                    break;
                }
            }
        }

        //Move buds to stem, and replenish new buds
        stemCells.UnionWith(budCells);
        budCells = newGrowth; //this very possibly might not do what i want
    }
}