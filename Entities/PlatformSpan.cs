using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;

namespace Sprint2.Entities;

public class Platform : IDrawableObject
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

        int width = tilesWide * Consts.platformWidth;
        int height = tilesTall * Consts.platformWidth;

        Bounds = new Rectangle(x, y, width, height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceSprite = SourceRects.PlatformSourceRects[PlatformType];

        int size = Consts.platformWidth;

        for (int height = 0; height < Bounds.Height; height += size)
        {
            for (int width = 0; width < Bounds.Width; width += size)
            {
                var finalPlat = new Rectangle(Bounds.X + width, Bounds.Y + height, size, size);
                spriteBatch.Draw(Assets.platformSpritesheet, finalPlat, sourceSprite, Color.White);
            }
        }
    }
}