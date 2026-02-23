using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Advait;

public abstract class Plant : IPlantable
{
    protected Texture2D texture;
    protected Rectangle sourceRectangle;

    protected const int TILE_SIZE = 37;

    protected Point horizontalOrigin;
    protected Point patternOrigin;

    protected float timer = 0f;
    protected float interval = 0.2f;

    protected int horizontalStep = 0;
    protected int patternStep = 0;

    protected bool startPattern = false;

    public Plant(Texture2D texture, Point startPosition)
    {
        this.texture = texture;
        this.horizontalOrigin = startPosition;

        sourceRectangle = new Rectangle(335, 450, 37, 37);
    }

    public virtual void Update(GameTime gameTime)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (timer >= interval)
        {
            timer = 0f;

            if (!startPattern)
                horizontalStep++;
            else
                patternStep++;
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        // Draw horizontal growth
        for (int i = 0; i < horizontalStep; i++)
        {
            Rectangle target = new Rectangle(
                horizontalOrigin.X + i * TILE_SIZE,
                horizontalOrigin.Y,
                TILE_SIZE,
                TILE_SIZE);

            spriteBatch.Draw(texture, target, sourceRectangle, Color.White);
        }

        // Draw vertical pattern
        if (startPattern)
        {
            Point[] pattern = GetPatternOffsets();

            for (int i = 0; i < patternStep && i < pattern.Length; i++)
            {
                Rectangle target = new Rectangle(
                    patternOrigin.X + pattern[i].X * TILE_SIZE,
                    patternOrigin.Y + pattern[i].Y * TILE_SIZE,
                    TILE_SIZE,
                    TILE_SIZE);

                spriteBatch.Draw(texture, target, sourceRectangle, Color.White);
            }
        }
    }

    public void StartVerticalGrowth()
    {
        if (!startPattern)
        {
            startPattern = true;

            patternOrigin = new Point(
                horizontalOrigin.X + (horizontalStep - 1) * TILE_SIZE,
                horizontalOrigin.Y);
        }
    }

    // Each plant defines its own pattern
    protected abstract Point[] GetPatternOffsets();
}
