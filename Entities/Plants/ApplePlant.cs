using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class ApplePlant(CollisionManager collisionManager, (int, int) root) : Plant(collisionManager, Species.apple, root)
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
                if (!collisionManager.Blocks.Contains(newCellPos))
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