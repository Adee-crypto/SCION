using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Managers;
using Sprint2.Util;

namespace Sprint2.Entities.Plants;

public enum Species
{
    grass, apple, pineapple
};

public abstract class Plant
{
    public static BlockType SpeciesToBlockType(Species plant) => plant switch
    {
        Species.grass => BlockType.Grass,
        Species.apple => BlockType.Apple,
        Species.pineapple => BlockType.Pineapple,
        _ => throw new System.NotImplementedException(),
    };

    protected Species Species { get; }
    protected BlockList BudCells { get; set; } = new();
    protected BlockList StemCells { get; } = new();
    public BlockList Blocks => new BlockList().Union(BudCells).Union(StemCells);
    protected float Age { get; set; }
    protected int CellsGrown {get; set;}
    protected Ticker Ticker { get; }
    private readonly CollisionManager collisionManager;

    public Plant(CollisionManager collisionManager, Species species, (int, int) root)
    {
        this.collisionManager = collisionManager;
        Ticker = new(PlantUtil.SpeciesGrowTimes[species]);
        Species = species;
        BudCells.Add(root, SpeciesToBlockType(species));
    }

    public abstract void Update(GameTime gameTime);

    //handles all growing logic
    protected abstract void Grow();

    //returns if it can grow into newCellPos, then grows there
    protected bool TryGrow(BlockList newGrowth, (int, int) newCellPos) {
        if (!collisionManager.Blocks.Contains(newCellPos)) {
            newGrowth.Add(newCellPos, SpeciesToBlockType(Species));
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

    //FIX THIS TO ONLY BREAK IF PLAYER ACTUALLY TOUCHING BLOCK; work with collision?
    public bool TryRemoveCellBelow(Vector2 bottomCenter)
    {
        int cellX = (int)(bottomCenter.X / Consts.BlockWidth);
        int cellY = (int)(bottomCenter.Y / Consts.BlockWidth) + 1;
        if (StemCells.Contains((cellX, cellY)))
        {
            StemCells.Remove((cellX, cellY));
            return true;
        }
        else if (StemCells.Contains((cellX - 1, cellY)) && bottomCenter.X % Consts.BlockWidth < Consts.BlockWidth / 2f)
        {
            StemCells.Remove((cellX - 1, cellY));
            return true;
        }
        else if (StemCells.Contains((cellX + 1, cellY)) && bottomCenter.X % Consts.BlockWidth > Consts.BlockWidth / 2f)
        {
            StemCells.Remove((cellX + 1, cellY));
            return true;
        }
        return false;
    }
}