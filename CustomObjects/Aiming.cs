using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2;

public class Aiming
{
    private readonly Texture2D texture;
    private Vector2 position;

    public Aiming(Texture2D aimingTexture, Vector2 initialPosition)
    {
        texture = aimingTexture;
        position = initialPosition;
    }

    public void Update(Vector2 newPosition)
    {
        position = newPosition;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, Color.White);
    }
}