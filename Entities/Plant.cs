using System;
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

    public bool RemoveCellBelow(Vector2 bottomCenter)
    {
        int cellX = (int)(bottomCenter.X / PlantUtil.cellWidth);
        int cellY = (int)(bottomCenter.Y / PlantUtil.cellWidth);
        if (stem_cells.Contains((cellX, cellY))) {
            stem_cells.Remove((cellX, cellY));
            return true;
        } else if (stem_cells.Contains((cellX - 1, cellY)) || stem_cells.Contains((cellX + 1, cellY)))
        {
            float leftCenterX = PlantUtil.cellWidth * (cellX - 0.5f);
            float rightCenterX = PlantUtil.cellWidth * (cellX + 0.5f);
            float leftDist = Math.Abs(bottomCenter.X - leftCenterX);
            float rightDist = Math.Abs(bottomCenter.X - rightCenterX);
            if (leftDist < rightDist) stem_cells.Remove((cellX - 1, cellY));
            else stem_cells.Remove((cellX + 1, cellY));
            return true;
        }
        return false;
    }
}