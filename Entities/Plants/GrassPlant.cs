using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class GrassPlant(BlockManager blockManager, (int, int) root) : Plant(blockManager, root, Species.Grass)
{
    public override void Update(GameTime gameTime) {
        if (IsGrowing) for (int i = 0; i < Ticker.TicksPassed(gameTime); i++) Grow();
    }

    protected override void Grow() {
        List<(int, int)> oldBudCells = [.. BudCells];
        BudCells.Clear();

        foreach ((int x, int y) in oldBudCells) {
            MatureCell((x, y));
            if (IsGrowing) TryGrow(BudCells, (x, y - 1));
        }
    }
}