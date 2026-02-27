using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Util; //FOR TESTING, DELETE THIS
using System;
using System.Collections.Generic;

namespace Sprint2.Entities.Plants;

public enum Species { grass, apple, pineapple };

public class Plant(Species species, (int, int) root)
{
    private Species species = species;
    private HashSet<(int, int)> budCells = [root];
    private HashSet<(int, int)> stemCells = [];
    private float timeGrown;

    public void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.D1))
        { //FOR TESTING
            timeGrown += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (timeGrown >= PlantUtil.SpeciesGrowTimes[species])
            {
                timeGrown -= PlantUtil.SpeciesGrowTimes[species];
                Grow();
            }
        }
    }

    private void Grow()
    {
        HashSet<(int, int)> newGrowth = [];
        foreach ((int x, int y) in budCells)
        {
            foreach ((int dx, int dy) in Funcs.ListShuffle(PlantUtil.growDirs))
            {
                (int, int) newCell = (x + dx, y + dy);
                if (!stemCells.Contains(newCell) && !budCells.Contains(newCell))
                {
                    newGrowth.Add(newCell);
                    break;
                }
            }
        }

        //Move buds to stem, and replenish new buds
        stemCells.UnionWith(budCells);
        budCells = newGrowth; //this very possibly might not do what i want
    }

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
        foreach ((int x, int y) in stemCells)
        {
            spriteBatch.Draw(Assets.PlantSpritesheet, new Vector2(x, y) * Consts.cellWidth, SourceRects.SpeciesSourceRects[species], Color.Gray);
        }
        foreach ((int x, int y) in budCells)
        {
            spriteBatch.Draw(Assets.PlantSpritesheet, new Vector2(x, y) * Consts.cellWidth, SourceRects.SpeciesSourceRects[species], Color.White);
        }
    }

    //Just a test method, prob won't be used in final game
    public void ToggleSpecies()
    {
        species = species switch
        {
            Species.grass => Species.apple,
            Species.apple => Species.pineapple,
            Species.pineapple => Species.grass,
            _ => species, // never happen
        };
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