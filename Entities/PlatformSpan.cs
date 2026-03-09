using Microsoft.Xna.Framework.Graphics;

namespace Sprint2.Entities;

public class Platform : Extensions.IDrawable
{
    public BlockList Blocks { get; } = new();

    public Platform(BlockType type, int x, int y, int w, int h)
    {
        for (int j = 0; j < h; j++)
        {
            for (int i = 0; i < w; i++)
            {
                Blocks.Add((x + i, y + j), type);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Blocks.Draw(spriteBatch);
    }
}