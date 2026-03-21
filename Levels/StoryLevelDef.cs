using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Sprint2.Entities.Plants;
using Sprint2.Managers;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Levels;

public sealed class StoryLevelDef
{
    public int Index { get; init; }
    public Vector2 PlayerSpawnPos { get; init; } = new Vector2(5, 5);
    public List<(BlockType, int, int, int, int)> Platforms { get; init; } = [];
    public List<Func<BlockManager, Plant>> Plants { get; init; } = [];
}