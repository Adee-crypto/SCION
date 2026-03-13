using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Entities.Projectiles;
using Sprint2.Extensions;
using Sprint2.Levels;

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

    public static Dictionary<Species, Func<BaseLevel, (int, int), Plant>> SpeciesToPlantInit { get; } = new() {
        { Species.Grass, (c, r) => new GrassPlant(c, r) },
        { Species.Apple, (c, r) => new ApplePlant(c, r) },
        { Species.Pineapple, (c, r) => new PineapplePlant(c, r) },
        { Species.Sandbox, (c, r) => new SandboxPlant(c, r) },
    };

    protected Species Species { get; }
    protected (int x, int y) Root {get;}
    protected BlockList BudCells { get; set; } = new();
    protected BlockList StemCells { get; } = new();
    protected float Age { get; set; }
    protected int CellsGrown {get; set;}
    protected int MaxCells {get; set;} = int.MaxValue;
    protected Ticker Ticker { get; }
    private readonly BaseLevel level; //terrible for coupling, fix somehow

    public Plant(BaseLevel level, Species species, (int, int) root)
    {
        this.level = level;
        Species = species;
        Root = root;
        Ticker = new(PlantUtil.SpeciesGrowTimes[species]);
        BudCells.Add(root, SpeciesToBlock[species]);
        level.AddBlockList(BudCells);
        level.AddBlockList(StemCells);
    }

    public abstract void Update(GameTime gameTime);

    //handles all growing logic
    protected abstract void Grow();

    //returns if it can grow into newCellPos, then grows there
    protected bool TryGrow(BlockList newGrowth, (int, int) newCellPos) {
        if (!level.HasBlockAt(newCellPos) && CellsGrown < MaxCells) {
            newGrowth.Add(newCellPos, SpeciesToBlock[Species]);
            CellsGrown++;
            return true;
        }
        return false;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        StemCells.Draw(spriteBatch, Color.Gray);
        BudCells.Draw(spriteBatch);
    }
}