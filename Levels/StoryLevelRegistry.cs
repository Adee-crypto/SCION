using System;
using Microsoft.Xna.Framework;
using Sprint2.Entities;

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
                new(BlockType.StoneBrick, 0, 16 * 25, 50, 1),
            ]
        },
        new StoryLevelDef
        {
            Index = 1,
            PlayerSpawnPos = new Vector2(5, 10),
            Platforms =
            [
                new Platform(BlockType.CrackedStoneBrick, 0, 16 * 20, 50, 1),
                new Platform(BlockType.Stone, 10 * 16, 16 * 15, 8, 1),
            ]
        }
    ];

    public static StoryLevelDef Get(int index)
    {
        return All[Math.Clamp(index, 0, All.Length-1)];
    }
}