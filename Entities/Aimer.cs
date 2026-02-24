using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint2.Util;
using System;

namespace Sprint2.Entities;

public class Aimer(float distanceFromPlayer = 20f)
{
    public Vector2 Direction { get; set; } = new(1, 0); // Default aim to the right
    public float Angle { get; set; } // Angle in radians
    public float DistanceFromPlayer { get; set; } = distanceFromPlayer;

    public void Update(Vector2 playerCenter, MouseState mouse)
    {
        Vector2 mousePos = new (mouse.X, mouse.Y);
        Vector2 aimerPos = mousePos - playerCenter;
        
        if (aimerPos.LengthSquared() > 0.0001f) Direction = Vector2.Normalize(aimerPos);

        Angle = (float) Math.Atan2(Direction.Y, Direction.X);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 playerCenter)
    {
        Vector2 position = playerCenter + Direction * DistanceFromPlayer;
        Vector2 origin = new (0, Assets.arrowTexture.Height / 2f);

        spriteBatch.Draw(Assets.arrowTexture, position, null, Color.White, Angle, origin, 1f, SpriteEffects.None, 0f);
    }
}