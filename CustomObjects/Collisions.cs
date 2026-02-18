using Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint2;

public class Collisions
{
    public static void ManageCollision(IPlayer player, IEnumerable<Rectangle> objects, Vector2 movement, ref bool isGrounded, ref Vector2 velocity)
    {
        Vector2 currentPosition = player.Position;

        currentPosition.X += movement.X;
        player.Position = currentPosition;
        foreach (var obj in objects)
        {
            if (!player.Hitbox.Intersects(obj)) continue;
            if (movement.X > 0)
                currentPosition.X = obj.Left - player.Hitbox.Width;
            else if (movement.X < 0)
                currentPosition.X = obj.Right;
        }

        currentPosition.Y += (int)movement.Y;
        player.Position = currentPosition;
        foreach (var obj in objects)
        {
            if (!player.Hitbox.Intersects(obj)) continue;
            if (player.Hitbox.Bottom - (int)movement.Y <= obj.Top) // Landing on top
            {
                isGrounded = true;
                currentPosition.Y = obj.Top - player.Hitbox.Height;
                velocity.Y = 0;
            }
            else if (player.Hitbox.Top - (int)movement.Y >= obj.Bottom) // Hitting ceiling
            {
                currentPosition.Y = obj.Bottom;
                velocity.Y = 0;
            }
            else // Pushed upward
            {
                currentPosition.Y = obj.Top - player.Hitbox.Height;
            }
        }

        player.Position = currentPosition;
    }

    public static bool CheckGrounded(IPlayer player, IEnumerable<Rectangle> objects, ref Vector2 movement)
    {
        Rectangle onePixelBelow = player.Hitbox;
        onePixelBelow.Y += 1;
        foreach (var obj in objects)
        {
            if (onePixelBelow.Intersects(obj))
            {
                movement.Y = obj.Top - player.Hitbox.Bottom;
                return true; 
            }
        }
        return false;
    }
}