using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2;

public class Platform
{
    public enum Type
    {
        stone,
        stonebrick,
        cracked_stonebrick
    }

    public Type PlatformType { get; }
    public Rectangle Bounds { get; private set; }

    public Platform(Type type, int x, int y, int tilesWide, int tilesTall)
    {
        PlatformType = type;

        int width = tilesWide * PlatformUtil.platformWidth;
        int height = tilesTall * PlatformUtil.platformWidth;

        Bounds = new Rectangle(x, y, width, height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceSprite = PlatformUtil.PlatformSpriteRects[PlatformType];

        int size = PlatformUtil.platformWidth;

        for (int height = 0; height < Bounds.Height; height += size)
        {
            for (int width = 0; width < Bounds.Width; width += size)
            {
                var finalPlat = new Rectangle(Bounds.X + width, Bounds.Y + height, size, size);
                spriteBatch.Draw(PlatformUtil.spritesheet, finalPlat, sourceSprite, Color.White);
            }
        }
    }
}