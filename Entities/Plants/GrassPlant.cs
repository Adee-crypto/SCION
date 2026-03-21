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
        if (BudCells.Count > 0) {
            (int x, int y) = BudCells[0];
            BudCells.Clear();
            TryMatureCell((x, y));
            TryGrow((x, y - 1));
        }
    }
}