using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Projectiles;
using Sprint2.Extensions;
using Sprint2.Levels;
using Sprint2.Managers;
using static Sprint2.Managers.BlockManager.Block;

namespace Sprint2.Entities.Plants;

public enum Species
{
    Grass, Apple, Pineapple, Sandbox
};

public abstract class Plant
{
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

    protected Species Species { get; }
    protected (int x, int y) Root {get;}
    protected HashSet<(int x, int y)> BudCells { get; set; } = [];
    protected HashSet<(int x, int y)> StemCells { get; } = [];
    protected float Age { get; set; }
    protected int CellsGrown {get; set;}
    protected int MaxCells {get; set;} = int.MaxValue;
    protected Ticker Ticker { get; }
    private readonly BlockManager blockManager;

    /// <summary>root MUST be free in blockManager</summary>
    public Plant(BlockManager blockManager, (int, int) root, Species species)
    {
        this.blockManager = blockManager;
        Species = species;
        Root = root;
        Ticker = new(PlantUtil.SpeciesGrowTimes[species]);
        BudCells.Add(root);
        blockManager.Add(root, SpeciesToBlock[species]);
    }

    public abstract void Update(GameTime gameTime);

    //handles all growing logic
    protected abstract void Grow();

    //returns if it can grow into newCellPos, then grows there
    protected bool TryGrow(HashSet<(int, int)> newGrowth, (int, int) newCellPos) {
        if (!blockManager.HasBlockAt(newCellPos) && CellsGrown < MaxCells) {
            newGrowth.Add(newCellPos);
            blockManager.Add(newCellPos, SpeciesToBlock[Species]);
            CellsGrown++;
            return true;
        }
        return false;
    }
}