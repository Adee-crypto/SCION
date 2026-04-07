//using Microsoft.Xna.Framework;
//using Sprint2.Extensions;
//using Sprint2.Managers;
//using Sprint2.Util;

//namespace Sprint2.Entities.Plants;

//// Static plant that does not grow. Amplifies nearby plant effects within 7 tiles left/right.
//// Effects: bigger bombs, faster vines, wider spread and so on

//public class CatalystFlowerPlant : Plant
//{
//    public const int InfluenceRange = 7;

//    public CatalystFlowerPlant(BlockManager blockManager, (int, int) root)
//        : base(blockManager, root, Species.Catalyst)
//    {
//        MaxCells = 1;
//        IsGrowing = false;

//        // Immediately mature as a single decorative block
//        BlockManager.Set(root, BlockType.Catalyst);
//        BlockManager.SetColor(root, Color.Gold);
//        StemCells.Add(root);
//    }

//    public override void Update(GameTime gameTime)
//    {
//        // No growth. This plant is purely passive.
//        // Other plants should query GetAmplificationFactor() when growing/spreading.
//    }

//    protected override void Grow()
//    {
//        // Does nothing - Catalyst never grows
//        //Although we could change this
//    }

//    /// Call this from other plants (Apple, Pineapple, Sandbox, Void, etc.) to get boost

//    public static float GetAmplificationFactor(BlockManager bm, (int x, int y) pos, string effectType = "default")
//    {
//        for (int dx = -InfluenceRange; dx <= InfluenceRange; dx++)
//        {
//            var checkPos = (pos.x + dx, pos.y);
//            if (bm.HasBlockAt(checkPos) && bm.BlockAt(checkPos).Type == BlockType.Catalyst)
//            {
//                return effectType switch
//                {
//                    "bomb" => 1.6f,
//                    "vine" => 1.8f,
//                    "spread" => 2.0f,
//                    _ => 1.3f
//                };
//            }
//        }
//        return 1.0f;
//    }
//}


//   // In ApplePlant.Grow(), PineapplePlant.Grow(), SandboxPlant.Grow(), etc.,

//    //float amp = CatalystFlowerPlant.GetAmplificationFactor(BlockManager, currentPos, "spread"); // or "vine", "bomb"