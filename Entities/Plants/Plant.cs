using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Util;
using System.Collections.Generic;

namespace Sprint2.Entities.Plants;

public enum Species { grass, apple, pineapple };

public abstract class Plant(Species species, (int, int) root)
{
    protected Species Species { get; } = species;
    protected HashSet<(int, int)> BudCells { get; set; }= [root];
    protected HashSet<(int, int)> StemCells { get; } = [];
    protected float Age { get; set; }
    protected Ticker Ticker { get; }= new(PlantUtil.SpeciesGrowTimes[species]);

    public abstract void Update(GameTime gameTime);

    protected abstract void Grow();

    public IEnumerable<Rectangle> GetPlantObjects()
    {
        foreach (var (x, y) in StemCells)
        {
            yield return new Rectangle(x * Consts.cellWidth, y * Consts.cellWidth, Consts.cellWidth, Consts.cellWidth);
        }

        foreach (var (x, y) in BudCells)
        {
            yield return new Rectangle(x * Consts.cellWidth, y * Consts.cellWidth, Consts.cellWidth, Consts.cellWidth);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var (x, y) in StemCells)
        {
            spriteBatch.Draw(Assets.PlantSpritesheet, new Vector2(x, y) * Consts.cellWidth, SourceRects.SpeciesSourceRects[Species], Color.Gray);
        }
        foreach (var (x, y) in BudCells)
        {
            spriteBatch.Draw(Assets.PlantSpritesheet, new Vector2(x, y) * Consts.cellWidth, SourceRects.SpeciesSourceRects[Species], Color.White);
        }
    }

    //FIX THIS TO ONLY BREAK IF PLAYER ACTUALLY TOUCHING BLOCK; work with collision?
    public bool TryRemoveCellBelow(Vector2 bottomCenter) {
        int cellX = (int)(bottomCenter.X / Consts.cellWidth);
        int cellY = (int)(bottomCenter.Y / Consts.cellWidth);
        if (StemCells.Contains((cellX, cellY))) { 
            StemCells.Remove((cellX, cellY));
            return true;
        } else if (StemCells.Contains((cellX - 1, cellY)) && bottomCenter.X % Consts.cellWidth < Consts.cellWidth / 2f) {
            StemCells.Remove((cellX - 1, cellY));
            return true;
        } else if (StemCells.Contains((cellX + 1, cellY)) && bottomCenter.X % Consts.cellWidth > Consts.cellWidth / 2f) {
            StemCells.Remove((cellX + 1, cellY));
            return true;
        }
        return false;
    }
}