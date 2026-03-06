using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Util;
using System.Collections.Generic;
using System.Linq;

namespace Sprint2.Entities.Plants;

public enum Species {
    grass, apple, pineapple
    };

public abstract class Plant
{
    public static BlockType SpeciesToBlockType(Species plant) => plant switch {
        Species.grass => BlockType.Grass,
        Species.apple => BlockType.Apple,
        Species.pineapple => BlockType.Pineapple,
        _ => throw new System.NotImplementedException(),
    };

    protected Species Species { get; }
    protected BlockList BudCells { get; set; } = new();
    protected BlockList StemCells { get; } = new();
    protected float Age { get; set; }
    protected Ticker Ticker { get; }

    public Plant(Species species, (int, int) root) {
        Ticker = new(PlantUtil.SpeciesGrowTimes[species]);
        Species = species;
        BudCells.Add(root, SpeciesToBlockType(species));
    }

    public abstract void Update(GameTime gameTime);

    protected abstract void Grow();

    public IEnumerable<Rectangle> GetPlantObjects()
    {
        return StemCells.ColliderBounds().Concat(BudCells.ColliderBounds());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        StemCells.Draw(spriteBatch, Color.Gray);
        BudCells.Draw(spriteBatch);
    }

    //FIX THIS TO ONLY BREAK IF PLAYER ACTUALLY TOUCHING BLOCK; work with collision?
    public bool TryRemoveCellBelow(Vector2 bottomCenter) {
        int cellX = (int)(bottomCenter.X / Consts.BlockWidth);
        int cellY = (int)(bottomCenter.Y / Consts.BlockWidth);
        if (StemCells.Contains((cellX, cellY))) { 
            StemCells.Remove((cellX, cellY));
            return true;
        } else if (StemCells.Contains((cellX - 1, cellY)) && bottomCenter.X % Consts.BlockWidth < Consts.BlockWidth / 2f) {
            StemCells.Remove((cellX - 1, cellY));
            return true;
        } else if (StemCells.Contains((cellX + 1, cellY)) && bottomCenter.X % Consts.BlockWidth > Consts.BlockWidth / 2f) {
            StemCells.Remove((cellX + 1, cellY));
            return true;
        }
        return false;
    }
}