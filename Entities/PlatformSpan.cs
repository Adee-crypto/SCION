using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;

namespace Sprint2.Entities;

public class Platform : Interfaces.IDrawable
{
    public enum Type
    {
        stone,
        stonebrick,
        crackedstonebrick
    }

    public Type PlatformType { get; }
    public Rectangle Bounds { get; private set; }

    public Platform(Type type, int x, int y, int tilesWide, int tilesTall)
    {
        PlatformType = type;

        int width = tilesWide * Consts.platformWidth;
        int height = tilesTall * Consts.platformWidth;

        Bounds = new(x, y, width, height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceSprite = SourceRects.PlatformSourceRects[PlatformType];

        int size = Consts.platformWidth;

        for (int height = 0; height < Bounds.Height; height += size)
        {
            for (int width = 0; width < Bounds.Width; width += size)
            {
                Rectangle finalPlat = new(Bounds.X + width, Bounds.Y + height, size, size);
                spriteBatch.Draw(Assets.PlatformSpritesheet, finalPlat, sourceSprite, Color.White);
            }
        }
    }
}