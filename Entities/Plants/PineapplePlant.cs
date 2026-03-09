using System;
using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class PineapplePlant(CollisionManager collisionManager, (int, int) root) : Plant(collisionManager, Species.pineapple, root)
{
    private readonly int maxCells = new Random().Next(7, 40);
    private (int x, int y) root = root;

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
                int parity = (x+root.x+y+root.y) % 2;
                if (parity == 0) {
                    TryGrow(newGrowth, (x+1, y));
                    TryGrow(newGrowth, (x-1, y));
                } else {
                    TryGrow(newGrowth, (x, y-1));
                    TryGrow(newGrowth, (x, y+1));
                }
            }
        }

        //Move buds to stem, and replenish new buds
        StemCells.Union(BudCells);
        BudCells = newGrowth;
    }
}