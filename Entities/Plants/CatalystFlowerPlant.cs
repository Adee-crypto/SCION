using Microsoft.Xna.Framework;
using Sprint2.Managers;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Entities.Plants;

public class CatalystFlowerPlant : Plant
{
    public const int InfluenceRange = 7;

    public CatalystFlowerPlant(BlockManager blockManager, (int, int) root)
    : base(blockManager, root, Species.Catalyst)
    {
        IsGrowing = false;

        // Immediately mature as a single decorative block
        BlockManager.Set(root, BlockType.Catalyst);
        BlockManager.SetColor(root, Color.Gold);
        StemCells.Add(root);
    }

    public override void Update(GameTime gameTime)
    {
        // No growth. This plant is purely passive.
    }

    protected override void Grow()
    {
        // Does nothing - Catalyst never grows
    }

    public static float GetAmplificationFactor(BlockManager bm, (int x, int y) pos, string effectType = "default")
    {
        for (int dx = -InfluenceRange; dx <= InfluenceRange; dx++)
        {
            var checkPos = (pos.x + dx, pos.y);
            if (bm.HasBlockAt(checkPos) && bm.BlockAt(checkPos).Type == BlockType.Catalyst)
            {
                return effectType switch
                {
                    "bomb" => 1.6f,
                    "vine" => 1.8f,
                    "spread" => 2.0f,
                    _ => 1.3f
                };
            }
        }
        return 1.0f;
    }
}