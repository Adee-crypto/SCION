using Microsoft.Xna.Framework;
using Sprint2.Entities;
using Sprint2.Util;

namespace Sprint2.Managers;

public class CollisionManager
{
    public BlockList Blocks {get;} = new();

    public void Reset() => Blocks.Clear();

    //CURRENTLY ASSUMES PLAYER IS LESS THAN THE WIDTH AND HEIGHT OF A BLOCK
    public (bool isCollision, bool isGrounded) ManageCollision(Collider collider, Vector2 deltaPos)
    {
        //*t*olerance
        float t = 0.001f;
        bool isGrounded = false, isCollision = false;

        //Collide Y
        collider.SetPositionY(collider.Position.Y + deltaPos.Y);
        int leftIndex = (int) ((collider.Left+t) / Consts.BlockWidth);
        int rightIndex = (int) ((collider.Right-t) / Consts.BlockWidth);
        int topIndex = (int) ((collider.Top+t) / Consts.BlockWidth);
        int bottomIndex = (int) ((collider.Bottom-t) / Consts.BlockWidth);
        // Console.WriteLine($"({leftIndex}, {topIndex}) to ({rightIndex}, {bottomIndex})");
        if (Blocks.Contains((leftIndex, bottomIndex)) || Blocks.Contains((rightIndex, bottomIndex))) {
            isGrounded = true;
            isCollision = true;
            collider.SetVelocityY(0);
            collider.SetPositionY(bottomIndex * Consts.BlockWidth - collider.Size.Y - t);
        } else if (Blocks.Contains((leftIndex, topIndex)) || Blocks.Contains((rightIndex, topIndex))) {
            isCollision = true;
            collider.SetVelocityY(0);
            collider.SetPositionY((topIndex+1) * Consts.BlockWidth + t);
        }

        //Collide X
        collider.SetPositionX(collider.Position.X + deltaPos.X);
        leftIndex = (int) ((collider.Left+t) / Consts.BlockWidth);
        rightIndex = (int) ((collider.Right-t) / Consts.BlockWidth);
        topIndex = (int) ((collider.Top+t) / Consts.BlockWidth);
        bottomIndex = (int) ((collider.Bottom-t) / Consts.BlockWidth);
        if (Blocks.Contains((rightIndex, topIndex)) || Blocks.Contains((rightIndex, bottomIndex))) {
            isCollision = true;
            collider.SetVelocityX(0);
            collider.SetPositionX(rightIndex * Consts.BlockWidth - collider.Size.X - t);
        } else if (Blocks.Contains((leftIndex, topIndex)) || Blocks.Contains((leftIndex, bottomIndex))) {
            isCollision = true;
            collider.SetVelocityX(0);
            collider.SetPositionX((leftIndex+1) * Consts.BlockWidth + t);
        }

        return (isCollision, isGrounded);
    }
}