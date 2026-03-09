using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Sprint2.Entities;
using Sprint2.Util;

namespace Sprint2.Managers;

public class CollisionManager
{
    public List<BlockList> Blocks {get;} = [];

    public bool HasBlockAt((int, int) pos) => Blocks.Any(b => b.Contains(pos));
    //checks that the block is breakable before breaking it
    public bool TryBreakBlockAt((int, int) pos) {
        foreach (var blockList in Blocks) {
            if (blockList.Contains(pos) && BlockList.IsBreakable(blockList.TypeAt(pos))) {
                return blockList.Remove(pos);
            }
        }
        return false;
    }
    public bool TryRemoveBlockAt((int, int) pos) => Blocks.Any(b => b.Remove(pos));
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
        if (HasBlockAt((leftIndex, bottomIndex)) || HasBlockAt((rightIndex, bottomIndex))) {
            isGrounded = true;
            isCollision = true;
            collider.SetVelocityY(0);
            collider.SetPositionY(bottomIndex * Consts.BlockWidth - collider.Size.Y - t);
        } else if (HasBlockAt((leftIndex, topIndex)) || HasBlockAt((rightIndex, topIndex))) {
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
        if (HasBlockAt((rightIndex, topIndex)) || HasBlockAt((rightIndex, bottomIndex))) {
            isCollision = true;
            collider.SetVelocityX(0);
            collider.SetPositionX(rightIndex * Consts.BlockWidth - collider.Size.X - t);
        } else if (HasBlockAt((leftIndex, topIndex)) || HasBlockAt((leftIndex, bottomIndex))) {
            isCollision = true;
            collider.SetVelocityX(0);
            collider.SetPositionX((leftIndex+1) * Consts.BlockWidth + t);
        }

        return (isCollision, isGrounded);
    }
}