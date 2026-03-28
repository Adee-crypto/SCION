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
        const int size = 12;
        const int half = size / 2;

        // One-shot: Instantaneous like a bomb
        for (int dy = 0; dy < size; dy++)
        {
            int y = Root.y - dy;                    // grow upward
            for (int dx = -half; dx < half; dx++)   // centered horizontally (±6)
            {
                int x = Root.x + dx;
                (int, int) cell = (x, y);

                if (!BlockManager.HasBlockAt(cell))
                {
                    // Add the block instantly (white, no animation)
                    BlockManager.Add(cell, PlantUtil.SpeciesToBlock[Species]);

                    // Add sound effects
                }
            }
        }

        // Fully grown
        CellsGrown = size * size;   // 144
        MatureAllBuds();            
        IsGrowing = false;
    }
}