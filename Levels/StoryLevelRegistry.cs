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
    //Move to consts or something?
    private const int gridCols = 4, gridRows = 4, levelW = 40, levelH = 30;
    private static readonly BlockType[] biomeBlocks = [BlockType.Muck, BlockType.Dirt, BlockType.Snow];
    private static readonly Species[] biomeSpecies = [Species.Apple, Species.Grass, Species.Pineapple];
    private static readonly (float bot, float top)[] bands = [(0, 0.3f), (0.6f, 1.3f), (1.7f, 4f)];

    private static Dictionary<(int, int), StoryLevelDef> Levels { get; } = [];
    public static bool Contains((int, int) coords) => Levels.ContainsKey(coords);
    public static StoryLevelDef Get((int,int) coords) => Levels[coords];

    private static int GenBiomeIndex(int cellRow) {
        float alt = gridRows - cellRow*1f/levelH;
        for (int i = 0; i < bands.Length; i++) {
            (var b, var t) = bands[i];
            if (alt < b && i != 0) {
                return Funcs.Random() < (alt - bands[i-1].top)/(b - bands[i-1].top) ? i : i-1;
            } else if (alt < t) {
                return i;
            }
        }
        return bands.Length-1;
    }

    public static void LoadLevelData()
    {
        string[] lines = File.ReadAllLines("Content/StoryMap.csv");
        string[][] cells = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            cells[i] = lines[i].Split(',');
        }

        for (int gr = 0; gr < gridRows; gr++)
        {
            for (int gc = 0; gc < gridCols; gc++)
            {
                (int, int) coords = (gc, gr);
                List<(BlockType, int, int)> blocks = [];
                List<(Species, int, int)> plants = [];
                Vector2 spawnPos = Vector2.Zero;

                for (int y = 0; y < levelH; y++)
                {
                    int cellRow = gr * levelH + y;
                    for (int x = 0; x < levelW; x++)
                    {
                        int cellCol = gc * levelW + x;
                        int localX = x, localY = y;
                        string cell = cells[cellRow][cellCol]?.Trim() ?? "";

                        switch (cell)
                        {
                            case "0": // player spawn
                                spawnPos = new Vector2(x, y) * Consts.BlockWidth;
                                break;

                            case "d": // dirt
                                blocks.Add((BlockType.Dirt, x, y));
                                break;

                            case "m": // muck ? treat as Dirt for now (or change to BlockType.Muck if you prefer)
                                blocks.Add((BlockType.Dirt, x, y));
                                break;

                            case "r": // stone + possible soil layer + rare plant
                                blocks.Add((BlockType.Stone, x, y));

                                if (cellRow > 0 && string.IsNullOrEmpty(cells[cellRow - 1][cellCol]))
                                {
                                    int soilHeight = Math.Min(Funcs.RandInt(3, 5), cellRow);
                                    if (soilHeight > 0)
                                    {
                                        for (int i = 0; i < soilHeight; i++)
                                        {
                                            int biomeIndex = GenBiomeIndex(cellRow);
                                            BlockType soilType = biomeBlocks[biomeIndex];
                                            blocks.Add((soilType, x, y - i - 1));

                                            // Plant on top of highest soil block
                                            if (i == soilHeight - 1 && Funcs.Random() < 0.15f)
                                            {
                                                Species species = biomeSpecies[biomeIndex];
                                                plants.Add((species, localX, localY - soilHeight));
                                            }
                                        }
                                    }
                                }
                                break;

                            case "a": // apple plant
                                plants.Add((Species.Apple, localX, localY));
                                break;

                            case "g": // grass plant
                                plants.Add((Species.Grass, localX, localY));
                                break;

                            default:
                                break;
                        }
                    }
                }

                Levels[coords] = new StoryLevelDef(
                    coords,
                    spawnPos,
                    [.. blocks],
                    [.. plants]
                );
            }
        }
    }
}