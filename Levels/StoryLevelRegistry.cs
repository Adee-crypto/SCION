using Microsoft.Xna.Framework;
using Sprint2.Entities;
using System;

namespace Sprint2.Levels;

public static class StoryLevelRegistry
{
    public static readonly StoryLevelDef[] All =
    [
        new StoryLevelDef
        {
            Index = 0,
            PlayerSpawnPos = new(5, 10),
            Platforms =
            [
                ((l) => new(l, BlockType.StoneBrick, 0, 25, 50, 1)),
            ]
        },
        new StoryLevelDef
        {
            Index = 1,
            PlayerSpawnPos = new Vector2(5, 10),
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