using Microsoft.Xna.Framework;
using Sprint2.Entities;
using System.Collections.Generic;

namespace Sprint2.Managers;

public class CollisionManager
{
    public List<Rectangle> Objects {get;} = [];

    public void Reset() => Objects.Clear();

    public void ManageCollision(Collider collider, Vector2 deltaPos)
    {
        Vector2 currentPosition = collider.Position;

        currentPosition.X += deltaPos.X;
        collider.SetPosition(currentPosition);

        foreach (var obj in Objects)
        {
            if (collider.Hitbox.Intersects(obj)) {
                if (deltaPos.X > 0) currentPosition.X = obj.Left - collider.Hitbox.Width;
                else if (deltaPos.X < 0) currentPosition.X = obj.Right;
            }
        }

        currentPosition.Y += deltaPos.Y;
        collider.SetPosition(currentPosition);
        foreach (var obj in Objects)
        {
            if (collider.Hitbox.Intersects(obj)) {
                if (deltaPos.Y < 0) collider.SetVelocityY(0);
                currentPosition.Y = (deltaPos.Y < 0) ? obj.Bottom : obj.Top - collider.Hitbox.Height;
            }
        }

        collider.SetPosition(currentPosition);
    }

    public bool CheckGrounded(Collider collider, ref Vector2 movement)
    {
        Rectangle onePixelBelow = collider.Hitbox;
        onePixelBelow.Y += 1;
        foreach (var obj in Objects)
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