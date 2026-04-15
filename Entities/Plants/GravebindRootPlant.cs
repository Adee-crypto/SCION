using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;
using System;
using System.Collections.Generic;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Entities.Plants;

public class GravebindRootPlant : Plant
{
    private (int x, int y) targetRoot;
    private float resistanceStrength;

    public GravebindRootPlant(BlockManager blockManager, (int, int) root)
    : base(blockManager, root, Species.Gravebind)
    {
        IsGrowing = true;
        FindHostRoot();
    }

    private void FindHostRoot()
    {
        targetRoot = Root;
        var visited = new HashSet<(int, int)>();
        var queue = new Queue<(int, int)>();
        queue.Enqueue(Root);

        while (queue.Count > 0)
        {
            var pos = queue.Dequeue();
            if (visited.Contains(pos)) continue;
            visited.Add(pos);

            if (BlockManager.HasBlockAt(pos) &&
                BlockManager.BlockAt(pos).Type != BlockType.Void &&
                pos.Item2 > targetRoot.Item2)   // .Item2 = y
            {
                targetRoot = pos;
            }

            foreach (var (dx, dy) in Consts.orthoDirs)
            {
                var n = (pos.Item1 + dx, pos.Item2 + dy);
                if (!visited.Contains(n) && BlockManager.HasBlockAt(n))
                    queue.Enqueue(n);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (IsGrowing)
        {
            int ticks = Ticker.TicksPassed(gameTime);
            for (int i = 0; i < ticks; i++)
                Grow();
        }

        ApplyVoidResistance();
    }

    protected override void Grow()
    {
        if (BudCells.Count == 0)
        {
            MatureAllBuds();
            IsGrowing = false;
            return;
        }

        (int x, int y) current = BudCells[0];
        BudCells.Clear();
        TryMatureCell(current);

        int dx = Math.Sign(targetRoot.x - current.x);
        int dy = Math.Sign(targetRoot.y - current.y);

        if (!TryGrowAlongHost((current.x + dx, current.y + dy)) &&
            !TryGrowAlongHost((current.x, current.y + dy)) &&
            !TryGrowAlongHost((current.x + dx, current.y)))
        {
            foreach (var dir in Funcs.ListShuffle(Consts.orthoDirs))
            {
                if (TryGrowAlongHost((current.x + dir.Item1, current.y + dir.Item2))) break;
            }
        }
    }

    private bool TryGrowAlongHost((int, int) newPos)
    {
        if (!BlockManager.HasBlockAt(newPos)) return false;
        if (BlockManager.BlockAt(newPos).Type == BlockType.Void) return false;

        if (TryGrow(newPos))
        {
            float distToRoot = MathF.Abs(newPos.Item1 - targetRoot.x) + MathF.Abs(newPos.Item2 - targetRoot.y);
            resistanceStrength = Math.Max(resistanceStrength, 1f / (1f + distToRoot * 0.5f));
            return true;
        }
        return false;
    }

    private void ApplyVoidResistance()
    {
        // TODO: Integrate with VoidPlant.Grow() later
    }
}