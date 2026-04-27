using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Entities.Plants;

public class VoidPlant(BlockManager blockManager, (int, int) root) : Plant(blockManager, root, Species.Void)
{
    public override void Update(GameTime gameTime)
    {
        for (int i = 0; i < Ticker.TicksPassed(gameTime); i++)
            Grow();
    }

    protected override void Grow()
    {
        // GravebindRootPlant resistance:
        List<(int, int)> oldBudCells = [.. BudCells];

        foreach ((int x, int y) in oldBudCells)
        {
            TryMatureCell((x, y));

            foreach ((int dx, int dy) in Consts.orthoDirs)
            {
                var newPos = (x + dx, y + dy);

                if (BlockManager.HasBlockAt(newPos) &&
                    !BudCells.Contains(newPos) &&
                    BlockManager.BlockAt(newPos).Type != BlockType.Void)
                {
                    float resistance = GetGravebindResistance(newPos);
                    float infectionChance = 1f - resistance;

                    // Only infect if random roll succeeds (resistance slows/stops spread)
                    if (Funcs.Random() < infectionChance)
                    {
                        BlockManager.Infect(newPos);
                        BudCells.Add(newPos);
                        Assets.PlantGrowthSFX(Species).Play(1, Funcs.SpeciesScale(Species), 0);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns resistance (0.0 to 1.0) at a position based on proximity to any Gravebind block.
    /// Uses the exact same formula as GravebindRootPlant: 1 / (1 + dist * 0.5)
    /// Closer to Gravebind = stronger resistance (lower infection chance).
    /// </summary>
    private float GetGravebindResistance((int, int) pos)
    {
        var visited = new HashSet<(int, int)>();
        var queue = new Queue<((int, int) position, float distance)>();
        queue.Enqueue((pos, 0f));
        visited.Add(pos);

        while (queue.Count > 0)
        {
            var (p, dist) = queue.Dequeue();

            if (dist > 15f) continue; // performance limit

            if (BlockManager.HasBlockAt(p) && BlockManager.BlockAt(p).Type == BlockType.Gravebind)
            {
                return 1f / (1f + dist * 0.5f);
            }

            foreach (var (dx, dy) in Consts.orthoDirs)
            {
                var neighbor = (p.Item1 + dx, p.Item2 + dy);
                if (visited.Add(neighbor)) queue.Enqueue((neighbor, dist + 1f));
            }
        }

        return 0f; // No nearby Gravebind = no resistance
    }
}