using Microsoft.Xna.Framework;
using Sprint2.Managers;
using Sprint2.Util;
using System;
using System.Collections.Generic;
using System.IO;
using Sprint2.Entities.Plants;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Levels;

public static class StoryLevelRegistry
{
    private static Dictionary<(int, int), StoryLevelDef> Levels { get; } = [];
    public static bool Contains((int, int) coords) => Levels.ContainsKey(coords);
    public static StoryLevelDef Get((int,int) coords) => Levels[coords];

    private static int GenBiomeIndex(int cellRow) {
        float alt = Consts.gridRows - cellRow*1f/Consts.levelH;
        for (int i = 0; i < Consts.bands.Length; i++) {
            (var b, var t) = Consts.bands[i];
            if (alt < b && i != 0) {
                return Funcs.Random() < (alt - Consts.bands[i-1].top)/(b - Consts.bands[i-1].top) ? i : i-1;
            } else if (alt < t) {
                return i;
            }
        }
        return Consts.bands.Length-1;
    }

    public static void LoadLevelData()
    {
        string[] lines = File.ReadAllLines("Content/StoryMap.csv");
        string[][] cells = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++) {
            cells[i] = lines[i].Split(',');
        }

        //Iterate through level coordinates
        for (int gr = 0; gr < Consts.gridRows; gr++) {
            for (int gc = 0; gc < Consts.gridCols; gc++) {
                (int, int) coords = (gc, gr);
                List<(BlockType, int, int, int, int)> platforms = [];
                List<Func<BlockManager, Plant>> plants = [];
                Vector2 spawnPos = Vector2.Zero;

                // Iterate through level's cells
                for (int y = 0; y < Consts.levelH; y++) {
                    int cellRow = gr * Consts.levelH + y;
                    for (int x = 0; x < Consts.levelW; x++) {
                        int cellCol = gc * Consts.levelW + x;
                        int localX = x, localY = y; //yeah theres probably a better way to do this
                        string cell = cells[cellRow][cellCol];
                        switch (cell) {
                            case "0": //player initial position
                                spawnPos = new Vector2(x, y) * Consts.BlockWidth;
                                break;
                            case "m": //muck block
                                platforms.Add((BlockType.Muck, x, y, 1, 1));
                                break;
                            case "d": //dirt block
                                platforms.Add((BlockType.Dirt, x, y, 1, 1));
                                break;
                            case "s": //dirt block
                                platforms.Add((BlockType.Snow, x, y, 1, 1));
                                break;
                            case "r": //stone block
                                platforms.Add((BlockType.Stone, x, y, 1, 1));

                                //biome generation logic when bare rock is detected
                                if (cellRow > 0 && cells[cellRow-1][cellCol].Length == 0) {
                                    int soilHeight = Math.Min(Funcs.RandInt(3, 5), cellRow);
                                    if (soilHeight > 0) {
                                        for (int i = 0; i < soilHeight; i++) {
                                            int biomeIndex = GenBiomeIndex(cellRow);
                                            platforms.Add((Consts.biomeBlocks[biomeIndex], x, y-i-1, 1, 1));
                                            if (i == soilHeight - 1 && Funcs.Random() < 0.15) {
                                                plants.Add(b => PlantUtil.SpeciesToPlantInit[Consts.biomeSpecies[biomeIndex]](b, (localX, localY-soilHeight)));                                
                                            }
                                        }
                                    }
                                }
                                break;
                            case "a": //apple plant
                                plants.Add(b => PlantUtil.SpeciesToPlantInit[Species.Apple](b, (localX, localY)));
                                break;
                            case "g": //grass plant
                                plants.Add(b => PlantUtil.SpeciesToPlantInit[Species.Grass](b, (localX, localY)));
                                break;
                            case "p": //pineapple plant
                                plants.Add(b => PlantUtil.SpeciesToPlantInit[Species.Pineapple](b, (localX, localY)));
                                break;
                            case "b": //bomb/sandbox plant
                                plants.Add(b => PlantUtil.SpeciesToPlantInit[Species.Sandbox](b, (localX, localY)));
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