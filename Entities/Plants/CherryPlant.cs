using System;
using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public class CherryPlant(BlockManager blockManager, (int, int) root) : Plant(blockManager, root, Species.Cherry)
{
    public override void Update(GameTime gameTime)
    {
        if (IsGrowing)
        {
            Grow();                    // Do the full growth in one step
            IsGrowing = false;         // Immediately stop
        }
    }

    protected override void Grow()
    {
        const int baseSize = 12;

        // Catalyst increases growth space (larger instant "bomb" area)
        float amp = CatalystFlowerPlant.GetAmplificationFactor(BlockManager, Root, "spread");
        int effectiveSize = (int)MathF.Ceiling(baseSize * amp);
        int half = effectiveSize / 2;

        // One-shot: Instantaneous large growth (like a bomb)
        for (int dy = 0; dy < effectiveSize; dy++)
        {
            int y = Root.y - dy;                    // grow upward
            for (int dx = -half; dx < half; dx++)   // centered horizontally
            {
                int x = Root.x + dx;
                (int, int) cell = (x, y);

                if (!BlockManager.HasBlockAt(cell))
                {
                    // Add the block instantly (white, no animation)
                    BlockManager.Add(cell, PlantUtil.SpeciesToBlock[Species]);

                    // TODO: Add sound effects here if desired
                }
            }
        }

        // Fully grown (scaled by catalyst)
        CellsGrown = effectiveSize * effectiveSize;
        MatureAllBuds();
        IsGrowing = false;
    }
}