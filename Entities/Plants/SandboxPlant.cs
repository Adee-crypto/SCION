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
        if (IsGrowing) {
            if (Ticker.TickAge >= 6) {
                BlockManager.SetColorAt(root, Color.White);
                Grow();
                IsGrowing = true;
            } else if (ticks > 0){ //colors swap upon a new tick
                BlockManager.SetColorAt(root, Ticker.TickAge % 2 == 0 ? Color.White : Color.Gray);
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

        //Breadth first search to grow as much as possible into desired shape from root
        List<(int, int)> StemDeltaPos = [];
        List<(int, int)> BudDeltaPos = [offset];
        while (BudDeltaPos.Count > 0) {
            (int x, int y) = BudDeltaPos[0];
            BudDeltaPos.RemoveAt(0);
            StemDeltaPos.Add((x,y));
            foreach ((int dx, int dy) in PlantUtil.GrowDirs) {
                (int x, int y) nextBud = (dx+x,dy+y);
                if (!BlockManager.HasBlockAt(nextBud) && nextBud.y >= 0 && nextBud.y < width && nextBud.x >= 0 && nextBud.x < width && pattern[nextBud.y][nextBud.x] == 1 && TryGrow(StemCells, (root.x+nextBud.x-offset.x,root.y+nextBud.y-offset.y))) {
                    BudDeltaPos.Add(nextBud);
                }
            }
        }
    }
}