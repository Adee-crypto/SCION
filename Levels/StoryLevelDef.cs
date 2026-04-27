using Microsoft.Xna.Framework;
using Sprint2.Entities.Plants;
using Sprint2.Managers;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Levels;

public readonly record struct StoryLevelDef(
    (int, int) Coords,
    Vector2 PlayerSpawnPos,
    (BlockType Type, int X, int Y)[] Blocks,   // Much simpler than rectangles
    (Species Species, int X, int Y)[] Plants   // Simple data instead of Func<>
);