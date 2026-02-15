using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Util;

public class Plant(PlantConst.Species species, Texture2D spritesheet, (int, int) root)
{
    private PlantConst.Species species = species;
    private Texture2D spritesheet = spritesheet;
    private HashSet<(int, int)> bud_cells = [root];
    private HashSet<(int, int)> stem_cells = [];

    public void Update(GameTime gameTime)
    {
        HashSet<(int, int)> newGrowth = [];
        foreach ((int, int) bud in bud_cells)
        {
            (int x, int y) = bud;
            int delta = new Random().Next(2) * 2 - 1;
            int axis = new Random().Next(2);
            if (axis == 1)
            {
                x += delta;
            }
            else
            {
                y += delta;
            }

            if (!stem_cells.Contains((x, y)) && !bud_cells.Contains((x, y)))
            {
                newGrowth.Add((x, y));
            }
        }

        //Move buds to stem, and replenism new buds
        stem_cells.UnionWith(bud_cells);
        bud_cells = newGrowth; //this very possibly might not do what i want
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(spritesheet, new Vector2(100, 100), PlantConst.SpeciesSpriteRects[species], Color.White);
    }
}