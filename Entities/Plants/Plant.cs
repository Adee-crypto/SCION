using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Lib;
using Sprint2.Util;
using System.Collections.Generic;

namespace Sprint2.Entities.Plants;

public enum Species { grass, apple, pineapple };

public abstract class Plant(Species species, (int, int) root)
{
    protected readonly Species species = species;
    protected HashSet<(int, int)> budCells = [root];
    protected HashSet<(int, int)> stemCells = [];
    protected float age;
    protected readonly Ticker ticker = new(PlantUtil.SpeciesGrowTimes[species]);

    public abstract void Update(GameTime gameTime);

    protected abstract void Grow();

    public IEnumerable<Rectangle> GetPlantObjects()
    {
        foreach (var (x, y) in stemCells)
        {
            yield return new Rectangle(x * Consts.cellWidth, y * Consts.cellWidth, Consts.cellWidth, Consts.cellWidth);
        }

        foreach (var (x, y) in budCells)
        {
            yield return new Rectangle(x * Consts.cellWidth, y * Consts.cellWidth, Consts.cellWidth, Consts.cellWidth);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var (x, y) in stemCells)
        {
            spriteBatch.Draw(Assets.PlantSpritesheet, new Vector2(x, y) * Consts.cellWidth, SourceRects.SpeciesSourceRects[species], Color.Gray);
        }
        foreach (var (x, y) in budCells)
        {
            spriteBatch.Draw(Assets.PlantSpritesheet, new Vector2(x, y) * Consts.cellWidth, SourceRects.SpeciesSourceRects[species], Color.White);
        }
    }

    //FIX THIS TO ONLY BREAK IF PLAYER ACTUALLY TOUCHING BLOCK; work with collision?
    public bool TryRemoveCellBelow(Vector2 bottomCenter) {
        int cellX = (int)(bottomCenter.X / Consts.cellWidth);
        int cellY = (int)(bottomCenter.Y / Consts.cellWidth);
        if (stemCells.Contains((cellX, cellY))) { 
            stemCells.Remove((cellX, cellY));
            return true;
        } else if (stemCells.Contains((cellX - 1, cellY)) && bottomCenter.X % Consts.cellWidth < Consts.cellWidth / 2f) {
            stemCells.Remove((cellX - 1, cellY));
            return true;
        } else if (stemCells.Contains((cellX + 1, cellY)) && bottomCenter.X % Consts.cellWidth > Consts.cellWidth / 2f) {
            stemCells.Remove((cellX + 1, cellY));
            return true;
        }
        return false;
    }
}