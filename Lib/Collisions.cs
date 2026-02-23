using Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint2;

public class Collisions
{
    public static void ManageCollision(IPhysicsObject entity, IEnumerable<Rectangle> objects, Vector2 movement, ref Vector2 velocity)
    {
        Vector2 currentPosition = entity.Position;

        currentPosition.X += movement.X;
        entity.Position = currentPosition;
        foreach (var obj in objects)
        {
            if (!entity.Hitbox.Intersects(obj)) continue;
            if (movement.X > 0) currentPosition.X = obj.Left - entity.Hitbox.Width;
            else if (movement.X < 0) currentPosition.X = obj.Right;
        }

        currentPosition.Y += movement.Y;
        entity.Position = currentPosition;
        foreach (var obj in objects)
        {
            if (!entity.Hitbox.Intersects(obj)) continue;
            if (movement.Y < 0) velocity.Y = 0;
            currentPosition.Y = (movement.Y < 0) ? obj.Bottom : obj.Top - entity.Hitbox.Height;
        }

        entity.Position = currentPosition;
    }

    public static bool CheckGrounded(IPhysicsObject entity, IEnumerable<Rectangle> objects, ref Vector2 movement)
    {
        Rectangle onePixelBelow = entity.Hitbox;
        onePixelBelow.Y += 1;
        foreach (var obj in objects)
        {
            if (onePixelBelow.Intersects(obj) && onePixelBelow.Bottom >= obj.Top)
            {
                movement.Y = obj.Top - entity.Hitbox.Bottom;
                return true; 
            }
        }
        return false;
    }
}