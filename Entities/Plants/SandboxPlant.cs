using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class SandboxPlant(BlockManager blockManager, (int, int) root) : Plant(blockManager, root, Species.Sandbox)
{
    private (int x, int y) root = root;

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
        int ticks = Ticker.TicksPassed(gameTime);
        if (IsGrowing && ticks > 0) {
            if (Ticker.TickAge < 6) { //colors swaps each tick
                BlockManager.SetColorAt(root, Ticker.TickAge % 2 == 0 ? Color.White : Color.Gray);
            } else {
                BlockManager.SetColorAt(root, Color.White);
                Grow();
                IsGrowing = false;
            }
        }
    }

    protected override void Grow()
    {
        //Pick which pattern with equal probability
        int[][] pattern = patterns[Funcs.RandInt(patterns.Length)];
        // randomize offset
        int width = pattern[0].Length;
        (int x, int y) offset = (Funcs.RandInt((width-1)/2, width/2+1), Funcs.RandInt((width-1)/2, width/2+1));

        //Breadth first search to grow as much as possible into desired shape from root
        StemCells.Clear();
        BudCells.Clear();
        //This is also lying and filling StemCells and BudCells with offset positions based on the root and pattern size
        BudCells.Add(offset);
        while (BudCells.Count > 0) {
            (int x, int y) = BudCells[0];
            BudCells.RemoveAt(0);
            foreach ((int dx, int dy) in PlantUtil.GrowDirs) {
                (int x, int y) nextBud = (dx+x,dy+y);
                if (!BlockManager.HasBlockAt(nextBud) && nextBud.y >= 0 && nextBud.y < width && nextBud.x >= 0 && nextBud.x < width && pattern[nextBud.y][nextBud.x] == 1 && TryGrow(StemCells, (root.x + nextBud.x - offset.x, root.y + nextBud.y - offset.y))) {
                    BudCells.Add(nextBud);
                }
            }
        }
    }
}