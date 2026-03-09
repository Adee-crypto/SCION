using Microsoft.Xna.Framework;
using Sprint2.Entities;
using Sprint2.Util;
using System;

namespace Sprint2.Levels;

public static class StoryLevelRegistry
{
    public static readonly StoryLevelDef[] All =
    [
        new StoryLevelDef
        {
            Index = 0,
            PlayerSpawnPos = new Vector2(25, 19)*Consts.BlockWidth,
            Platforms =
            [
                ((l) => new(l, BlockType.Dirt, 23, 20, 5, 5)),
                ((l) => new(l, BlockType.CrackedStoneBrick, 30, 25, 20, 1)),
            ]
        },
        new StoryLevelDef
        {
            Index = 1,
            PlayerSpawnPos = new Vector2(5, 10)*Consts.BlockWidth,
            Platforms =
            [
                ((l) => new(l, BlockType.CrackedStoneBrick, 0, 20, 50, 1)),
                ((l) => new(l, BlockType.Stone, 10, 15, 8, 1)),
            ]
        }
    ];

    public static StoryLevelDef Get(int index)
    {
        return All[Math.Clamp(index, 0, All.Length - 1)];
    }
}