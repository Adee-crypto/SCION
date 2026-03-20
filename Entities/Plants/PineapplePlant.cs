using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Levels;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class PineapplePlant : Plant
{
    public PineapplePlant(BlockManager blockManager, (int, int) root) : base(blockManager, root, Species.Pineapple) {
        MaxCells = Funcs.RandInt(7, 40);
    }
    
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
        if (CellsGrown < MaxCells) { //slightly redundant since TryGrow also checks this
            foreach ((int x, int y) in BudCells)
            {
                //calculates parity
                if ((x+Root.x+y+Root.y) % 2 == 0) {
                    TryGrow(newGrowth, (x+1, y));
                    TryGrow(newGrowth, (x-1, y));
                } else {
                    TryGrow(newGrowth, (x, y-1));
                    TryGrow(newGrowth, (x, y+1));
                }
            }
        }

        //Move buds to stem, and replenish new buds
        StemCells.UnionWith(BudCells);
        BudCells.Clear();
        BudCells.UnionWith(newGrowth);
    }
}