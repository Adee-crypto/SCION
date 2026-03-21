using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using Sprint2.Entities.Plants;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Levels;

public static class StoryLevelRegistry
{
    private static List<StoryLevelDef> Levels { get; } = [];

    public static void LoadLevelData() {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string filePath = Path.Combine(baseDir, "Content", "StoryLevelData.csv");
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i += 4) {

            int index = int.Parse(lines[i].Trim(), CultureInfo.InvariantCulture);

            string[] spawnParts = lines[i + 1].Split(',');
            Vector2 spawnPos = new Vector2(
                float.Parse(spawnParts[0], CultureInfo.InvariantCulture), 
                float.Parse(spawnParts[1], CultureInfo.InvariantCulture)
            ) * Consts.BlockWidth;


            List<(BlockType, int, int, int, int)> platforms = [];
            string[] platformParts = lines[i + 2].Split(',');
            for (int p = 0; p < platformParts.Length; p += 5)
            {
                string typeStr = platformParts[p].Trim();
                int x = int.Parse(platformParts[p + 1], CultureInfo.InvariantCulture);
                int y = int.Parse(platformParts[p + 2], CultureInfo.InvariantCulture);
                int w = int.Parse(platformParts[p + 3], CultureInfo.InvariantCulture);
                int h = int.Parse(platformParts[p + 4], CultureInfo.InvariantCulture);

                if (Enum.TryParse(typeStr, out BlockType bType))
                {
                    platforms.Add((bType, x, y, w, h));
                }
            }

            List<Func<BlockManager, Plant>> plants = [];
            string[] plantParts = lines[i + 3].Split(',');
            for (int p = 0; p < plantParts.Length; p += 3)
            {
                string typeStr = plantParts[p].Trim();
                int x = int.Parse(plantParts[p + 1], CultureInfo.InvariantCulture);
                int y = int.Parse(plantParts[p + 2], CultureInfo.InvariantCulture);

                if (Enum.TryParse(typeStr, out Species species))
                {
                    plants.Add((b) => species switch { //TODO fix this with a dict from Plant.cs
                        Species.Grass => new GrassPlant(b, (x, y)),
                        Species.Apple => new ApplePlant(b, (x, y)),
                        Species.Pineapple => new PineapplePlant(b, (x, y)),
                        Species.Sandbox => new SandboxPlant(b, (x, y)),
                        _ => throw new ArgumentException($"Unknown species: {species}")
                    });
                }
            }

            Levels.Add(new(index, spawnPos, [.. platforms], [.. plants]));
        }
    }

    public static StoryLevelDef Get(int index) => Levels[Math.Clamp(index, 0, Levels.Count - 1)];
}