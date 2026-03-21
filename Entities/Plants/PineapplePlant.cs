using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class PineapplePlant(BlockManager blockManager, (int, int) root) : Plant(blockManager, root, Species.Pineapple)
{
    public override void Update(GameTime gameTime)
    {
        if (IsGrowing) for (int i = 0; i < Ticker.TicksPassed(gameTime); i++) Grow();
    }

    protected override void Grow()
    {
        int budCellIndex = BudCells.Count - 1 - (int) MathF.Floor(MathF.Log(1+Funcs.Random()*(MathF.Pow(2, BudCells.Count)-1), 2));
        (int x, int y) = BudCells[budCellIndex];
        BudCells.RemoveAt(budCellIndex);
        MatureCell((x, y));
        
        if (BudCells.Count > 0) {
            int toggle = Funcs.PlusMinus();
            //calculates parity
            if ((x+Root.x+y+Root.y) % 2 == 0) {
                TryGrow(BudCells, (x+toggle, y));
                TryGrow(BudCells, (x-toggle, y));
            } else {
                TryGrow(BudCells, (x, y-toggle));
                TryGrow(BudCells, (x, y+toggle));
            }
        } else {
            IsGrowing = false;
            BudCells.ForEach(MatureCell);
        }
    }
}