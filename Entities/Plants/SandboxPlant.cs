using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Sprint2.Levels;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class SandboxPlant(BaseLevel level, (int, int) root) : Plant(level, Species.sandbox, root)
{
    private (int x, int y) root = root;
    private bool detonated;

    private readonly (int,int)[] pattern = [
        (0, 0),
        (0, -1),
        (-1, 0),
        (1, 0),
        (0, 1),
        (0, -2),
        (-1, -1),
        (1, -1),
        (-2, 0),
        (2, 0),
        (-1, 1),
        (1, 1),
        (0, 2),
        (-1, -2),
        (1, -2),
        (-2, -1),
        (2, -1),
        (-2, 1),
        (2, 1),
        (-1, 2),
        (1, 2),
    ];

    public override void Update(GameTime gameTime)
    {
        if (!detonated && Ticker.TicksPassed(gameTime) > 0)
        {
            Grow();
            detonated = true;
        }
    }

    protected override void Grow()
    {
        BudCells.Clear();

        List<(int, int)> StemDeltaPos = [];
        List<(int, int)> BudDeltaPos = [(0,0)];


        Console.WriteLine(pattern);
        while (BudDeltaPos.Count > 0) {
            (int x, int y) = BudDeltaPos[0];
            StemDeltaPos.Add((x,y));
            BudDeltaPos.RemoveAt(0);
            if (TryGrow(StemCells, (root.x+x,root.y+y))) {
                foreach ((int dx, int dy) in PlantUtil.GrowDirs) {
                    (int, int) nextBud = (dx+x,dy+y);
                    Console.WriteLine($"{nextBud}");
                    if (!StemDeltaPos.Contains(nextBud) && !BudCells.Contains(nextBud) && pattern.Contains(nextBud)) {
                        BudDeltaPos.Add(nextBud);
                    }
                }
            }
        }
    }
}