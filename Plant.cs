using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Util;

public class Plant
{
    private PlantConst.Species species;
    private Texture2D spritesheet;

    public Plant(PlantConst.Species species, Texture2D spritesheet)
    {
        this.species = species;
        this.spritesheet = spritesheet;
    }

    public void Update(GameTime gameTime)
    {
        // Plants are static, so no update logic is needed for now.
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(spritesheet, new Vector2(100, 100), PlantConst.SpeciesSpriteRects[species], Color.White);
    }
}