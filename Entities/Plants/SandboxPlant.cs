using Microsoft.Xna.Framework;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class SandboxPlant(BlockManager blockManager, (int, int) root) : Plant(blockManager, root, Species.Sandbox)
{
    /// <summary>
    /// Each pattern is a square binary grid defining the shape the plant tries to grow into.
    /// 1 = the plant will attempt to fill this cell, 0 = it won't.
    /// Patterns are centered on the root using a randomized offset so growth isn't always aligned the same way.
    /// </summary>
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
        if (IsGrowing && ticks > 0)
        {
            if (Ticker.TickAge < 6)
            { //colors swaps each tick
                BlockManager.SetColor(Root, Ticker.TickAge % 2 == 0 ? Color.White : Color.Gray);
            }
            else
            {
                BlockManager.SetColor(Root, Color.White);
                Grow();
                IsGrowing = false;
            }
        }
    }

    protected override void Grow()
    {
        int[][] pattern = PickPattern();
        (int x, int y) offset = RandomOffset(pattern[0].Length);
        ExpandIntoPattern(pattern, offset);
    }

    /// <summary>Returns one of the shape patterns at random.</summary>
    private int[][] PickPattern() => patterns[Funcs.RandInt(patterns.Length)];

    /// <summary>
    /// Returns a random offset used to loosely center the pattern on the root,
    /// so the growth isn't always aligned to the same corner.
    /// </summary>
    private static (int x, int y) RandomOffset(int width) =>
        (Funcs.RandInt((width - 1) / 2, width / 2 + 1),
         Funcs.RandInt((width - 1) / 2, width / 2 + 1));

    /// <summary>
    /// BFS from the current bud cells, growing into any neighbour that falls
    /// within a pattern cell marked 1.
    /// </summary>
    private void ExpandIntoPattern(int[][] pattern, (int x, int y) offset)
    {
        int width = pattern[0].Length;
        while (BudCells.Count > 0)
        {
            (int x, int y) = BudCells[0];
            BudCells.RemoveAt(0);
            foreach ((int dx, int dy) in Consts.orthoDirs)
            {
                (int x, int y) nextBud = (x + dx, y + dy);
                if (IsInPattern(nextBud, pattern, width, offset))
                    TryGrow(nextBud);
            }
        }
    }

    /// <summary>Returns true if a world cell falls on a pattern cell marked 1.</summary>
    private bool IsInPattern((int x, int y) cell, int[][] pattern, int width, (int x, int y) offset)
    {
        (int x, int y) patternPos = (cell.x - Root.x + offset.x, cell.y - Root.y + offset.y);
        return patternPos.y >= 0 && patternPos.y < width
            && patternPos.x >= 0 && patternPos.x < width
            && pattern[patternPos.y][patternPos.x] == 1;
    }
}