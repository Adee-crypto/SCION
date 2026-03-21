using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Managers;

namespace Sprint2.Entities.Plants;

public class VoidPlant(BlockManager blockManager, (int, int) root) : Plant(blockManager, root, Species.Void)
{
    public override void Update(GameTime gameTime) {
        for (int i = 0; i < Ticker.TicksPassed(gameTime); i++) Grow();
    }

    protected override void Grow()
    {
        List<(int, int)> oldBudCells = [.. BudCells];
        BudCells.Clear();
        
        foreach ((int x, int y) in oldBudCells) {
            MatureCell((x, y));
            foreach ((int dx, int dy) in PlantUtil.GrowDirs) {
                var newPos = (x + dx, y + dy);
                if (BlockManager.HasBlockAt(newPos)) {
                    BlockManager.Infect(newPos);
                    BudCells.Add(newPos);
                }
            }
        }
    }
}