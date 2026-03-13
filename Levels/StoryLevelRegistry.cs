using Microsoft.Xna.Framework;
using Sprint2.Entities;
using Sprint2.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Sprint2.Levels;

public static class StoryLevelRegistry
{
    public static List<StoryLevelDef> Levels {get; private set;} = [];

    public static void LoadLevelData() {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDir, "Content", "StoryLevelData.csv");
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i += 3) {

            int index = int.Parse(lines[i].Trim(), CultureInfo.InvariantCulture);

            string[] spawnParts = lines[i + 1].Split(',');
            Vector2 spawnPos = new Vector2(
                float.Parse(spawnParts[0], CultureInfo.InvariantCulture), 
                float.Parse(spawnParts[1], CultureInfo.InvariantCulture)
            ) * Consts.BlockWidth;

            List<Func<BaseLevel, Platform>> platforms = [];
            string[] platformParts = lines[i + 2].Split(',');

            // Step through the array 5 items at a time
            for (int p = 0; p < platformParts.Length; p += 5)
            {
                // Capture local variables for the lambda closure
                string typeStr = platformParts[p].Trim();
                int x = int.Parse(platformParts[p + 1], CultureInfo.InvariantCulture);
                int y = int.Parse(platformParts[p + 2], CultureInfo.InvariantCulture);
                int w = int.Parse(platformParts[p + 3], CultureInfo.InvariantCulture);
                int h = int.Parse(platformParts[p + 4], CultureInfo.InvariantCulture);

                if (Enum.TryParse(typeStr, out BlockType bType))
                {
                    platforms.Add((l) => new Platform(l, bType, x, y, w, h));
                }
            }

            Levels.Add(new StoryLevelDef {
                Index = index,
                PlayerSpawnPos = spawnPos,
                Platforms = platforms
            });
        }
        // ((l) => new(l, BlockType.Dirt, 23, 20, 5, 5)),
        // ((l) => new(l, BlockType.CrackedStoneBrick, 30, 25, 20, 1)),
    }

    public static StoryLevelDef Get(int index)
    {
        return Levels[Math.Clamp(index, 0, Levels.Count - 1)];
    }
}