using Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint2;

public class Collisions
{
    public static void ManageCollision(IPlayer player, IEnumerable<Rectangle> objects, Vector2 movement, ref Vector2 velocity)
    {
        Vector2 currentPosition = player.Position;

        currentPosition.X += movement.X;
        player.Position = currentPosition;
        foreach (var obj in objects)
        {
            if (!player.Hitbox.Intersects(obj)) continue;
            if (movement.X > 0) currentPosition.X = obj.Left - player.Hitbox.Width;
            else if (movement.X < 0) currentPosition.X = obj.Right;
        }

        currentPosition.Y += movement.Y;
        player.Position = currentPosition;
        foreach (var obj in objects)
        {
            if (!player.Hitbox.Intersects(obj)) continue;
            if (movement.Y < 0) velocity.Y = 0;
            currentPosition.Y = (movement.Y < 0) ? obj.Bottom : obj.Top - player.Hitbox.Height;
        }

        player.Position = currentPosition;
    }

    public static bool CheckGrounded(IPlayer player, IEnumerable<Rectangle> objects, ref Vector2 movement)
    {
        Rectangle onePixelBelow = player.Hitbox;
        onePixelBelow.Y += 1;
        foreach (var obj in objects)
        {
            if (onePixelBelow.Intersects(obj) && onePixelBelow.Bottom >= obj.Top)
            {
                movement.Y = obj.Top - player.Hitbox.Bottom;
                return true; 
            }
        }
        return false;
    }
}