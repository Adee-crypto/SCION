using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Sprint2.Entities;
using Sprint2.Util;

namespace Sprint2.Managers;

public class CollisionManager
{
    public List<BlockList> Blocks {get;} = [];
    // *t*olerance
    private const float t = 0.001f;

    public bool HasBlockAt((int, int) pos) => Blocks.Any(b => b.Contains(pos));
    
    //checks that the block is breakable before breaking it
    public BlockType? TryBreakBlockAt((int, int) pos) {
        foreach (var blockList in Blocks) {
            if (blockList.Contains(pos)) {
                BlockType type = blockList.TypeAt(pos);
                if (BlockList.IsBreakable(type)) {
                    if (blockList.Remove(pos)) { //technically this check should be redundant but just in case
                        return type;
                    }
                }
            }
        }
        return null;
    }
    public bool TryRemoveBlockAt((int, int) pos) => Blocks.Any(b => b.Remove(pos));
    public void Reset() => Blocks.Clear();

    /// <summary>CURRENTLY ASSUMES COLLIDER IS LESS THAN THE WIDTH AND HEIGHT OF A BLOCK</summary>
    public bool IsCollision(Vector2 pos, Vector2 size) {
        int leftIndex = Funcs.GridCoord(pos.X+t);
        int rightIndex = Funcs.GridCoord(pos.X+size.X-t);
        int topIndex = Funcs.GridCoord(pos.Y+t);
        int bottomIndex = Funcs.GridCoord(pos.Y+size.Y-t);

        return HasBlockAt((leftIndex, topIndex)) || HasBlockAt((rightIndex, topIndex)) || HasBlockAt((leftIndex, bottomIndex)) || HasBlockAt((rightIndex, bottomIndex));
    }

    /// <summary>d is the dimension of the hitbox in that direction</summary>
    public float ShoveTowardOrigin(float pos, float d) {
        return Funcs.GridCoord(pos+d) * Consts.BlockWidth - d - t;
    }
    
    public float ShoveAwayOrigin(float pos) {
        return (Funcs.GridCoord(pos) + 1) * Consts.BlockWidth + t;
    }

    /// <summary>CURRENTLY ASSUMES COLLIDER IS LESS THAN THE WIDTH AND HEIGHT OF A BLOCK</summary>
    public (bool isCollision, bool isGrounded) ManageBlockCollision(Collider collider, Vector2 deltaPos) {
        
        Vector2 newPos = collider.Position + deltaPos;
        Vector2 size = collider.Size;
        //check if no collision
        if (!IsCollision(newPos, size)) {
            collider.SetPosition(newPos);
            return (false, false);
        }

        float leftX = ShoveTowardOrigin(newPos.X, size.X);
        float rightX = ShoveAwayOrigin(newPos.X);
        float topY = ShoveTowardOrigin(newPos.Y, size.Y);
        float bottomY = ShoveAwayOrigin(newPos.Y);
        SortedDictionary<Vector2, List<Vector2>> nearestSpots = new(new Funcs.VectorComparer<Vector2>(newPos)) {
            //orthogonal cases
            { new(newPos.X, topY),    [new(0, -Consts.BlockWidth)] },
            { new(newPos.X, bottomY), [new(0, Consts.BlockWidth)] },
            { new(leftX, newPos.Y),   [new(-Consts.BlockWidth, 0)] },
            { new(rightX, newPos.Y),  [new(Consts.BlockWidth, 0)] },};
            //diagonal cases take priority first in case of key collision
            nearestSpots[new(leftX, topY)] =     [new(0, -Consts.BlockWidth), new(-Consts.BlockWidth, 0)];
            nearestSpots[new(rightX, topY)] =    [new(0, -Consts.BlockWidth), new(Consts.BlockWidth, 0)];
            nearestSpots[new(leftX, bottomY)] =  [new(0, Consts.BlockWidth),  new(-Consts.BlockWidth, 0)];
            nearestSpots[new(rightX, bottomY)] = [new(0, Consts.BlockWidth),  new(Consts.BlockWidth, 0)];

        //TLDR glorified BFS where only certain directions are explored depending on the coords
        //Why: will *always* find the closest cell by euclidean distance
        while (true) {
            (var pos, var offsets) = nearestSpots.First();
            nearestSpots.Remove(pos);
            if (IsCollision(pos, size)) { //add neighbors via custom BFS directions defined upon initialization
                foreach (var offset in offsets) {
                    nearestSpots[pos+offset] = offsets; //would be nice if this wasn't overwriting data constantly but sometimes we don't get what we want
                }
            } else { //Free spot finally found
                if (offsets.Any(o => o.X == 0)) collider.SetVelocityY(0);
                if (offsets.Any(o => o.Y == 0)) collider.SetVelocityX(0);
                collider.SetPosition(pos);
                return (true, offsets.Any(o => o.Y < 0));
            }
        }
    }
}