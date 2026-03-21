using System;
using System.Collections.Generic;
using Sprint2.Entities.Projectiles;
using Sprint2.Managers;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Entities.Plants;

public static class PlantUtil
{
    public static List<(int, int)> GrowDirs { get; } = [(0, 1), (0, -1), (1, 0), (-1, 0)];

    //there are WAY too many of these
    public static Dictionary<Species, BlockType> SpeciesToBlock {get;} = new() {
        {Species.Grass, BlockType.Grass},
        {Species.Apple, BlockType.Apple},
        {Species.Pineapple, BlockType.Pineapple},
        {Species.Sandbox, BlockType.Sandbox},
    };

    public static Dictionary<BlockType, Species> BlockToSpecies {get;} = new() {
        {BlockType.Grass, Species.Grass},
        {BlockType.Apple, Species.Apple},
        {BlockType.Pineapple, Species.Pineapple},
        {BlockType.Sandbox, Species.Sandbox},
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
    };
    
    public static Dictionary<Species, float> SpeciesGrowTimes { get; } = new()
    {
        {Species.Grass, 0.1f },
        {Species.Apple, 0.2f },
        {Species.Pineapple, 0.4f },
        {Species.Sandbox, 0.2f }, //6 ticks until detonation
    };
}