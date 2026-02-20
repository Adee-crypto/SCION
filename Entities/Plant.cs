using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; //FOR TESTING, DELETE THIS

namespace Sprint2;

public class Plant(Plant.Species species, (int, int) root)
{
    public enum Species {grass, apple, pineapple};

    private Species species = species;
    private HashSet<(int, int)> bud_cells = [root];
    private HashSet<(int, int)> stem_cells = [];
    private float timeGrown = 0f;

    public void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.D1)) { //FOR TESTING
            timeGrown += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (timeGrown >= PlantUtil.SpeciesGrowTimes[species]) {
                timeGrown -= PlantUtil.SpeciesGrowTimes[species];
                Grow();
            }
        }
    }

    private void Grow()
    {
        HashSet<(int, int)> newGrowth = [];
        foreach ((int x, int y) in bud_cells)
        {
            foreach ((int dx, int dy) in PlantUtil.ShuffledDirs()) {
            (int, int) newCell = (x + dx, y + dy);
            if (!stem_cells.Contains(newCell) && !bud_cells.Contains(newCell))
                {
                    newGrowth.Add(newCell);
                    break;
                }
            }
        }

        //Move buds to stem, and replenish new buds
        stem_cells.UnionWith(bud_cells);
        bud_cells = newGrowth; //this very possibly might not do what i want
    }

    public IEnumerable<Rectangle> GetPlantObjects()
    {
        foreach (var (x, y) in stem_cells)
        {
            yield return new Rectangle(x * PlantUtil.cellWidth, y * PlantUtil.cellWidth, PlantUtil.cellWidth, PlantUtil.cellWidth);
        }

        foreach (var (x, y) in bud_cells)
        {
            yield return new Rectangle(x * PlantUtil.cellWidth, y * PlantUtil.cellWidth, PlantUtil.cellWidth, PlantUtil.cellWidth);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach ((int x, int y) in stem_cells)
        {
            spriteBatch.Draw(PlantUtil.spritesheet, new Vector2(x, y)*PlantUtil.cellWidth, PlantUtil.SpeciesSpriteRects[species], Color.Gray);
        }
        foreach ((int x, int y) in bud_cells)
        {
            spriteBatch.Draw(PlantUtil.spritesheet, new Vector2(x, y)*PlantUtil.cellWidth, PlantUtil.SpeciesSpriteRects[species], Color.White);
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

    /// test

    public (int, int)? FindClosestCellBelow(Vector2 position)
    {
        int gridX = (int)(position.X / PlantUtil.cellWidth);
        int gridY = (int)(position.Y / PlantUtil.cellWidth);

        (int, int)? best = null;
        int bestDist = int.MaxValue;

        foreach ((int cx, int cy) in stem_cells)
        {
            if (cx == gridX && cy >= gridY)
            {
                int dist = cy - gridY;
                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = (cx, cy);
                }
            }
        }
        foreach ((int cx, int cy) in bud_cells)
        {
            if (cx == gridX && cy >= gridY)
            {
                int dist = cy - gridY;
                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = (cx, cy);
                }
            }
        }
        return best;
    }

    public bool RemoveBlock(int cellX, int cellY)
    {
        if (stem_cells.Remove((cellX, cellY))) return true;
        if (bud_cells.Remove((cellX, cellY))) return true;
        return false;
    }
}