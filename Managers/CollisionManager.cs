using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Sprint2.Entities;
using Sprint2.Util;

namespace Sprint2.Managers;

public class CollisionManager(BlockManager blockManager)
{
    private readonly BlockManager blockManager = blockManager;
    // *t*olerance
    private const float t = 0.001f;

    public bool IsCollision(Collider c) => IsCollision(c.Position, c.Size);

    public bool IsCollision(Vector2 pos, Vector2 size) => GetCollisionCoords(pos, size).Count > 0;

    public HashSet<(int, int)> GetCollisionCoords(Vector2 pos, Vector2 size) {
        HashSet<(int, int)> output = [];
        int leftIndex = Funcs.GridCoord(pos.X+t);
        int rightIndex = Funcs.GridCoord(pos.X+size.X-t);
        int topIndex = Funcs.GridCoord(pos.Y+t);
        int bottomIndex = Funcs.GridCoord(pos.Y+size.Y-t);

        //checks all tiles intersecting with hitbox
        for (int y = topIndex; y <= bottomIndex; y++) {
            for (int x = leftIndex; x <= rightIndex; x++) {
                if (blockManager.HasBlockAt((x, y))) output.Add((x, y));
            }
        }

        return output;
    }

    public ((int, int)? collisionCoords, bool isGrounded) ManageBlockCollision(Collider collider, Vector2 deltaPos) {

        Vector2 newPos = collider.Position + deltaPos;
        Vector2 size = collider.Size;

        //check if no collision
        if (!IsCollision(newPos, size)) {
            collider.SetPosition(newPos);
            return (null, false);
        }

        float leftX = Funcs.ShoveTowardOrigin(newPos.X, size.X)-t;
        float rightX = Funcs.ShoveAwayOrigin(newPos.X)+t;
        float topY = Funcs.ShoveTowardOrigin(newPos.Y, size.Y)-t;
        float bottomY = Funcs.ShoveAwayOrigin(newPos.Y)+t;
        SortedDictionary<Vector2, List<Vector2>> nearestSpots = new(new Funcs.VectorComparer<Vector2>(newPos)) {
            //orthogonal cases
            { new(newPos.X, topY),    [new(0, -Consts.BlockWidth)] },
            { new(newPos.X, bottomY), [new(0, Consts.BlockWidth)] },
            { new(leftX, newPos.Y),   [new(-Consts.BlockWidth, 0)] },
            { new(rightX, newPos.Y),  [new(Consts.BlockWidth, 0)] },};
            //diagonal cases override in case of key collision
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
                return (Funcs.GridCoords(newPos), offsets.Any(o => o.Y < 0));
            }
        }
    }
}