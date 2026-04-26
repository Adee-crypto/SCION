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
        string[] lines = File.ReadAllLines("Content/StoryMap.csv");
        string[][] cells = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++) {
            cells[i] = lines[i].Split(',');
        }

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
                    int cellRow = gr * levelH + y;
                    for (int x = 0; x < levelW; x++) {
                        int cellCol = gc * levelW + x;
                        int localX = x, localY = y; //yeah theres probably a better way to do this
                        string cell = cells[cellRow][cellCol];
                        switch (cell) {
                            case "0": //player initial position
                                spawnPos = new Vector2(x, y) * Consts.BlockWidth;
                                break;
                            case "d": //dirt block
                                platforms.Add((BlockType.Dirt, x, y, 1, 1));
                                break;
                            case "m": //muck block
                                platforms.Add((BlockType.Dirt, x, y, 1, 1));
                                break;
                            case "r": //rock block
                                platforms.Add((BlockType.Stone, x, y, 1, 1));
                                if (y > 0 && cells[cellRow-1][cellCol].Length == 0) {
                                    for (int i = 0; i < Math.Min(Funcs.RandInt(2, 4), y); i++){
                                        platforms.Add((BlockType.Dirt, x, y-i-1, 1, 1));
                                    }
                                }
                                break;
                            case "a": //apple plant
                                plants.Add(b => PlantUtil.SpeciesToPlantInit[Species.Apple](b, (localX, localY)));
                                break;
                            case "g": //grass plant
                                plants.Add(b => PlantUtil.SpeciesToPlantInit[Species.Grass](b, (localX, localY)));
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