using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sprint2;

public class Collisions
{
    public static void ManageCollision(Player player, IEnumerable<Rectangle> objects, Vector2 movement)
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
            } else if (movement.X < 0)
            {
                currentPos.X = o.Right;
            } else if (movement.X == 0)
            {
                currentPos.X = o.Right >= (o.Left - player.Hitbox.Width) ? (o.Left - player.Hitbox.Width) : o.Right;
            }

            player.Position = currentPos;
        }

        currentPos.Y += movement.Y;
        player.Position = currentPos;

        foreach (var o in objects)
        {
            if (!player.Hitbox.Intersects(o)) continue;

            if (movement.Y > 0)
            {
                currentPos.Y = o.Top - player.Hitbox.Height;
            } else if (movement.Y < 0)
            {
                currentPos.Y = o.Bottom;
            } else if (movement.Y == 0)
            {
                currentPos.Y = o.Bottom >= (o.Top - player.Hitbox.Height) ? (o.Top - player.Hitbox.Height) : o.Bottom;
            }

            player.Position = currentPos;
        }
    }
}