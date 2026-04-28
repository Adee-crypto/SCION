using Microsoft.Xna.Framework;
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

        for (int gr = 0; gr < Consts.gridRows; gr++) {
            for (int gc = 0; gc < Consts.gridCols; gc++) {
                (int, int) coords = (gc, gr);
                List<(BlockType, int, int)> blocks = [];
                List<(Species, int, int)> plants = [];
                Vector2 spawnPos = Vector2.Zero;

                for (int y = 0; y < Consts.levelH; y++) {
                    int cellRow = gr * Consts.levelH + y;
                    for (int x = 0; x < Consts.levelW; x++) {
                        int cellCol = gc * Consts.levelW + x;

                        string cell = cells[cellRow][cellCol];
                        switch (cell) {
                            case "0": // player spawn
                                spawnPos = new Vector2(x, y) * Consts.BlockWidth;
                                break;
                            case "d": // dirt
                                blocks.Add((BlockType.Dirt, x, y));
                                break;
                            case "m": // muck
                                blocks.Add((BlockType.Muck, x, y));
                                break;
                            case "s": // snow
                                blocks.Add((BlockType.Snow, x, y));
                                break;
                            case "u": // up
                                blocks.Add((BlockType.Up, x, y));
                                break;
                            case "w": // warp
                                blocks.Add((BlockType.Warp, x, y));
                                break;
                            case "r": // rock/stone
                                blocks.Add((BlockType.Stone, x, y));
                                //soil layer
                                if (cellRow > 0 && string.IsNullOrEmpty(cells[cellRow - 1][cellCol]))
                                {
                                    int biomeIndex = GenBiomeIndex(cellRow);
                                    var (min, max) = Consts.topsoilRanges[biomeIndex];
                                    int soilHeight = Math.Min(Funcs.RandInt(min, max+1), cellRow);
                                    if (soilHeight > 0) {
                                        for (int i = 0; i < soilHeight; i++) {
                                            BlockType soilType = Consts.biomeBlocks[biomeIndex];

                                            int thisY = y - i - 1;
                                            if (thisY < 0) { //this repetition is bad, fix later
                                                Levels[(coords.Item1, coords.Item2-1)].Blocks.Add((soilType, x, thisY + Consts.levelH));

                                                // Plant on top of highest soil block
                                                if (i == soilHeight - 1 && Funcs.Random() < 0.15f)
                                                {
                                                    Species species = Consts.biomeSpecies[biomeIndex];
                                                    Levels[(coords.Item1, coords.Item2-1)].Plants.Add((species, x, y - soilHeight + Consts.levelH));
                                                }
                                            } else {
                                                blocks.Add((soilType, x, thisY));

                                                // Plant on top of highest soil block
                                                if (i == soilHeight - 1 && Funcs.Random() < 0.15f)
                                                {
                                                    Species species = Consts.biomeSpecies[biomeIndex];
                                                    plants.Add((species, x, y - soilHeight));
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            case "a": // apple plant
                                plants.Add((Species.Apple, x, y));
                                break;
                            case "g": // grass plant
                                plants.Add((Species.Grass, x, y));
                                break;
                            case "p": // pineapple
                                plants.Add((Species.Pineapple, x, y));
                                break;
                            case "b": // sandbox/bomb
                                plants.Add((Species.Sandbox, x, y));
                                break;
                            default:
                                break;
                        }
                    }
                }
                Levels[coords] = new(coords, spawnPos, blocks, plants);
            }
        }
    }
}