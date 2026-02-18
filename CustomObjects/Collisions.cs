using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Sprint2;

public class Collisions
{
    public static void ManageCollision(Player player, IEnumerable<Rectangle> objects, Vector2 movement, ref bool isGrounded, ref Vector2 velocity)
    {
        Vector2 currentPos = player.Position;
        currentPos.X += movement.X;
        player.Position = currentPos;

        foreach (var o in objects)
        {
            if (!player.Hitbox.Intersects(o)) continue;
            if (movement.X > 0)
            {
                currentPos.X = o.Left - player.Hitbox.Width;
            }
            else if (movement.X < 0)
            {
                currentPos.X = o.Right;
            }
            player.Position = currentPos;
        }

        float oldY = currentPos.Y;
        currentPos.Y += movement.Y;
        player.Position = currentPos;
        Rectangle newRect = player.Hitbox;
        Rectangle oldRect = new Rectangle(newRect.X, (int)oldY, newRect.Width, newRect.Height);

        foreach (var o in objects)
        {
            if (!newRect.Intersects(o)) continue;

            if (oldRect.Bottom <= o.Top) // Landing on top
            {
                isGrounded = true;
                currentPos.Y = o.Top - newRect.Height;
                velocity.Y = 0;
            }
            else if (oldRect.Top >= o.Bottom) // Hitting ceiling
            {
                currentPos.Y = o.Bottom;
                velocity.Y = 0;
            }
            else if (movement.Y > 0)
            {
                currentPos.Y = o.Top - newRect.Height;
            }
            else if (movement.Y < 0)
            {
                currentPos.Y = o.Bottom;
            }

            player.Position = currentPos;
            newRect = player.Hitbox;
        }
    }
}