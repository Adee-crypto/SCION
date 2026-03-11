using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Extensions;
using Sprint2.Levels;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class SandboxPlant(BaseLevel level, (int, int) root) : Plant(level, Species.sandbox, root)
{
    private (int x, int y) root = root;
    private bool detonated;

    private readonly int[][][] patterns = 
    [
    [
        [0, 1, 1, 0],
        [1, 1, 1, 1],
        [1, 1, 1, 1],
        [0, 1, 1, 0],
    ],
    [
        [0, 1, 1, 1, 0],
        [1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1],
        [0, 1, 1, 1, 0],
    ],
    [
        [0, 1, 1, 1, 0],
        [1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1],
        [0, 1, 1, 1, 0],
    ],
    [
        [0, 0, 1, 1, 0, 0],
        [0, 1, 1, 1, 1, 0],
        [1, 1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1, 1],
        [0, 1, 1, 1, 1, 0],
        [0, 0, 1, 1, 0, 0],
    ],
    ];

    public override void Update(GameTime gameTime)
    {
        Ticker.TicksPassed(gameTime);
        if (!detonated)
        {
            if (Ticker.TickAge >= 6) {
                Grow();
                detonated = true;
            } else if (Ticker.TickAge % 2 == 0) {
                StemCells.Union(BudCells);
                BudCells.Clear();
            } else {
                BudCells.Union(StemCells);
                StemCells.Clear();
            }
        }
    }

    protected override void Grow()
    {
        StemCells.Clear();
        BudCells.Clear();

        //Pick which pattern with equal probability
        int[][] pattern = patterns[Funcs.RandInt(patterns.Length)];
        // randomize offset
        int width = pattern[0].Length;
        (int x, int y) offset = (Funcs.RandInt((width-1)/2, width/2+1), Funcs.RandInt((width-1)/2, width/2+1));

        //Breadth first search to grow as much as possible into desired shape
        List<(int, int)> StemDeltaPos = [];
        List<(int, int)> BudDeltaPos = [offset];
        while (BudDeltaPos.Count > 0) {
            (int x, int y) = BudDeltaPos[0];
            BudDeltaPos.RemoveAt(0);
            StemDeltaPos.Add((x,y));
            if (TryGrow(StemCells, (root.x+x-offset.x,root.y+y-offset.y))) {
                foreach ((int dx, int dy) in PlantUtil.GrowDirs) {
                    (int x, int y) nextBud = (dx+x,dy+y);
                    if (!StemDeltaPos.Contains(nextBud) && !BudCells.Contains(nextBud) && nextBud.y >= 0 && nextBud.y < width && nextBud.x >= 0 && nextBud.x < width && pattern[nextBud.y][nextBud.x] == 1) {
                        BudDeltaPos.Add(nextBud);
                    }
                }
            }
        }
    }
}