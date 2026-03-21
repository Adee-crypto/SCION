using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class ApplePlant(BlockManager blockManager, (int, int) root) : Plant(blockManager, root, Species.Apple)
{
    private new readonly int MaxCells = Funcs.RandInt(10, 21);

    public override void Update(GameTime gameTime) {
        if (IsGrowing) for (int i = 0; i < Ticker.TicksPassed(gameTime); i++) Grow();
    }

    protected override void Grow()
    {
        HashSet<(int, int)> newGrowth = [];
        
        foreach ((int x, int y) in BudCells) {
            BlockManager.SetColorAt((x, y), Color.Gray);
            if (IsGrowing && CellsGrown < MaxCells) {
                foreach ((int dx, int dy) in Funcs.ListShuffle(PlantUtil.GrowDirs)) {
                    if (TryGrow(newGrowth, (x + dx, y + dy))) break;
                }
            } else IsGrowing = false;
        }
        
        //Move buds to stem, and replenish new buds
        StemCells.UnionWith(BudCells);
        BudCells.Clear();
        BudCells.UnionWith(newGrowth);

    }
}