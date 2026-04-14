using System;
using Microsoft.Xna.Framework;
using Sprint2.Entities.Plants;
using Sprint2.Managers;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Levels;

public readonly record struct StoryLevelDef(
    (int, int) Coords,
    Vector2 PlayerSpawnPos, 
    (BlockType, int, int, int, int)[] Platforms, 
    Func<BlockManager, Plant>[] Plants
);