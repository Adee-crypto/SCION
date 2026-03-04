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
            PlayerSpawnPos = new Vector2(5, 10),
            Platforms = 
            [
                new Platform(Platform.Type.stonebrick, 0, 16 * 25, 50, 1),
            ]
        },
        new StoryLevelDef
        {
            Index = 1,
            PlayerSpawnPos = new Vector2(5, 10),
            Platforms =
            [
                new Platform(Platform.Type.crackedstonebrick, 0, 16 * 20, 50, 1),
                new Platform(Platform.Type.stone, 10 * 16, 16 * 15, 8, 1),
            ]
        }
    ];

    public static StoryLevelDef Get(int index)
    {
        if (index < 0) index = 0;
        if (index >= All.Length) index = All.Length - 1;
        return All[index];
    }
}