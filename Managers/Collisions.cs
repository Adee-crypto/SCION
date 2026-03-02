using Microsoft.Xna.Framework;
using Sprint2.Entities;
using System.Collections.Generic;

namespace Sprint2.Managers;

public class CollisionManager
{
    public static void ManageCollision(Collider collider, IEnumerable<Rectangle> objects, Vector2 movement)
    {
        Vector2 currentPosition = collider.Position;

        currentPosition.X += movement.X;
        collider.SetPosition(currentPosition);

        foreach (var obj in objects)
        {
            if (collider.Hitbox.Intersects(obj)) {
                if (movement.X > 0) currentPosition.X = obj.Left - collider.Hitbox.Width;
                else if (movement.X < 0) currentPosition.X = obj.Right;
            }
        }

        currentPosition.Y += movement.Y;
        collider.SetPosition(currentPosition);
        foreach (var obj in objects)
        {
            if (collider.Hitbox.Intersects(obj)) {
                if (movement.Y < 0) collider.SetVelocityY(0);
                currentPosition.Y = (movement.Y < 0) ? obj.Bottom : obj.Top - collider.Hitbox.Height;
            }
        }

        collider.SetPosition(currentPosition);
    }

    public static bool CheckGrounded(Collider collider, IEnumerable<Rectangle> objects, ref Vector2 movement)
    {
        Rectangle onePixelBelow = collider.Hitbox;
        onePixelBelow.Y += 1;
        foreach (var obj in objects)
        {
            if (onePixelBelow.Intersects(obj) && onePixelBelow.Bottom >= obj.Top)
            {
                movement.Y = obj.Top - collider.Hitbox.Bottom;
                return true;
            }
        }
        return false;
    }
}