using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint2;

public class Projectile
{
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public bool IsAlive { get; private set; } = true;

    private readonly Texture2D texture;
    private readonly Vector2 size;

    public Rectangle Hitbox => new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);

    public Projectile(Texture2D objectTexture, Vector2 initialPosition, Vector2 velocity)
    {
        texture = objectTexture;
        Position = initialPosition;
        Velocity = velocity;
        size = new Vector2(texture.Width, texture.Height);
    }

    public void Update(GameTime gameTime)
    {
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public void Kill() => IsAlive = false;

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, Position, Color.White);
    }
}