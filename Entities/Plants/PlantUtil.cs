using System;
using System.Collections.Generic;
using Sprint2.Entities.Projectiles;
using Sprint2.Managers;
using Sprint2.Util;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Entities.Plants;

public static class PlantUtil
{
    public static readonly Species[] species = [Species.Apple, Species.Grass, Species.Pineapple, Species.Sandbox];

    public static Species RandomSpecies() => species[Funcs.RandInt(species.Length)];

    //there are WAY too many of these
    public static Dictionary<Species, BlockType> SpeciesToBlock {get;} = new() {
        {Species.Grass, BlockType.Grass},
        {Species.Apple, BlockType.Apple},s
        {Species.Pineapple, BlockType.Pineapple},
        {Species.Sandbox, BlockType.Sandbox},
        {Species.Void, BlockType.Void},
        { Species.Cherry, BlockType.Cherry },
    };

    public static Dictionary<BlockType, Species> BlockToSpecies {get;} = new() {
        {BlockType.Grass, Species.Grass},
        {BlockType.Apple, Species.Apple},
        {BlockType.Pineapple, Species.Pineapple},
        {BlockType.Sandbox, Species.Sandbox},
        { BlockType.Cherry, Species.Cherry },
    };

    public static Dictionary<Species, ProjectileType> SpeciesToProjectile {get;} = new() {
        {Species.Grass, ProjectileType.Grass},
        {Species.Apple, ProjectileType.Apple},
        {Species.Pineapple, ProjectileType.Pineapple},
        {Species.Sandbox, ProjectileType.Sandbox},

    };

    public static Dictionary<Species, Func<BlockManager, (int, int), Plant>> SpeciesToPlantInit { get; } = new() {
        { Species.Grass, (b, r) => new GrassPlant(b, r) },
        { Species.Apple, (b, r) => new ApplePlant(b, r) },
        { Species.Pineapple, (b, r) => new PineapplePlant(b, r) },
        { Species.Sandbox, (b, r) => new SandboxPlant(b, r) },
        { Species.Cherry, (b, r) => new CherryPlant(b, r) },
    };
    
    public static Dictionary<Species, float> SpeciesGrowTimes { get; } = new()
    {
        {Species.Grass, 0.1f },
        {Species.Apple, 0.2f },
        {Species.Pineapple, 0.4f },
        {Species.Sandbox, 0.2f }, //6 ticks until detonation
        {Species.Void, 1f },
        { Species.Cherry, 0f },
    };

    public static int SpeciesMaxCells(Species species) => species switch {
        Species.Grass => Funcs.RandInt(5, 8),
        Species.Apple => Funcs.RandInt(10, 21),
        Species.Pineapple => Funcs.RandInt(7, 40),
        _ => int.MaxValue,
        Species.Cherry => 144,
    };
}