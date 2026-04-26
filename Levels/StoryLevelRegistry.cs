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
    private static Dictionary<(int, int), StoryLevelDef> Levels { get; } = [];
    public static bool Contains((int, int) coords) => Levels.ContainsKey(coords);
    public static StoryLevelDef Get((int,int) coords) => Levels[coords];

    public static void LoadLevelData()
    {
        string[] lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "Content/StoryMap.csv");
        // TODO move to consts or just not be random idk

        int gridCols = 4;
        int gridRows = 4;
        int levelW = 40;
        int levelH = 30;
        //Iterate through level coordinates
        for (int gr = 0; gr < gridRows; gr++) {
            for (int gc = 0; gc < gridCols; gc++) {
                (int, int) coords = (gc, gr);
                List<(BlockType, int, int, int, int)> platforms = [];
                List<Func<BlockManager, Plant>> plants = [];
                Vector2 spawnPos = Vector2.Zero;

                // Iterate through level's cells
                for (int y = 0; y < levelH; y++) {
                    string[] rowData = lines[(gr * levelH) + y].Split(',');
                    for (int x = 0; x < levelW; x++) {
                        string cell = rowData[(gc * levelW) + x].Trim();
                        switch (cell) {
                            case "0": //player initial position
                                spawnPos = new Vector2(x, y) * Consts.BlockWidth;
                                break;
                            case "d": //dirt block
                                platforms.Add((BlockType.Dirt, x, y, 1, 1));
                                break;
                            case "m": //mud block
                                platforms.Add((BlockType.Dirt, x, y, 1, 1));
                                break;
                            case "r": //mud block
                                platforms.Add((BlockType.Stone, x, y, 1, 1));
                                break;
                            case "a":
                                int px = x; int py = y;
                                plants.Add(b => PlantUtil.SpeciesToPlantInit[Species.Apple](b, (px, py)));
                                break;
                            default:
                                break;
                        }
                    }
                }
                Levels[coords] = new(coords, spawnPos, [..platforms], [..plants]);
            }
        }
    }
}