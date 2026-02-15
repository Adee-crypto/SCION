using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2;

public class Plant(Plant.Species species, Texture2D spritesheet, (int, int) root)
{
    public enum Species {grass, apple, pineapple};

    private Species species = species;
    private Texture2D spritesheet = spritesheet;
    private HashSet<(int, int)> bud_cells = [root];
    private HashSet<(int, int)> stem_cells = [];

    public void Update(GameTime gameTime)
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

        //Move buds to stem, and replenism new buds
        stem_cells.UnionWith(bud_cells);
        bud_cells = newGrowth; //this very possibly might not do what i want
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach ((int x, int y) in stem_cells)
        {
            spriteBatch.Draw(spritesheet, new Vector2(x, y)*PlantUtil.cellWidth, PlantUtil.SpeciesSpriteRects[species], Color.Gray);
        }
        foreach ((int x, int y) in bud_cells)
        {
            spriteBatch.Draw(spritesheet, new Vector2(x, y)*PlantUtil.cellWidth, PlantUtil.SpeciesSpriteRects[species], Color.White);
        }
    }
}