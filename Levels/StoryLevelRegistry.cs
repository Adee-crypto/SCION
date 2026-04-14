using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using Sprint2.Entities.Plants;
using static Sprint2.Managers.BlockManager.Block;
using System.Linq;

namespace Sprint2.Levels;

public static class StoryLevelRegistry
{
    private static Dictionary<(int, int), StoryLevelDef> Levels { get; } = [];
    public static List<(int, int)> LevelCoords {get; private set; }

    public static void LoadLevelData() {
        //this probably shouldn't be finding the directory via Base like this
        string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "Content/StoryLevelData.csv");

        for (int i = 0; i < lines.Length; i += 4) {

            string[] coordsStr = lines[i].Split(',');
            (int, int) coords = (
                int.Parse(coordsStr[0], CultureInfo.InvariantCulture), 
                int.Parse(coordsStr[1], CultureInfo.InvariantCulture)
            );

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
                    plants.Add(b => PlantUtil.SpeciesToPlantInit[species](b, (x, y)));
                }
            }
            Console.WriteLine(coords);
            Levels[coords] = new(coords, spawnPos, [.. platforms], [.. plants]);
        }
        LevelCoords = [.. Levels.Keys];
    }

    public static StoryLevelDef? Get((int,int) coords) {
        if(Levels.TryGetValue(coords, out StoryLevelDef value)) return value;
        return null;
    }
}