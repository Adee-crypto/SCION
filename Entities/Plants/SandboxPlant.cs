using Microsoft.Xna.Framework;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class SandboxPlant(BlockManager blockManager, (int, int) root) : Plant(blockManager, root, Species.Sandbox)
{
    private readonly int[][][] patterns = [
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
                BlockManager.SetColor(root, Ticker.TickAge % 2 == 0 ? Color.White : Color.Gray);
            } else {
                BlockManager.SetColor(root, Color.White);
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
        while (BudCells.Count > 0) {
            (int x, int y) = BudCells[0];
            BudCells.RemoveAt(0);
            foreach ((int dx, int dy) in Consts.orthoDirs) {
                (int x, int y) nextBud = (x+dx, y+dy);
                (int x, int y) patternPos = (nextBud.x - Root.x + offset.x, nextBud.y - Root.y + offset.y);
                if (patternPos.y >= 0 && patternPos.y < width && patternPos.x >= 0 && patternPos.x < width && pattern[patternPos.y][patternPos.x] == 1)
                    TryGrow(nextBud);
            }
        }
    }
}