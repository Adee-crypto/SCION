using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Sprint2.Entities;
using Sprint2.Util;

namespace Sprint2.Managers;

public class CollisionManager(BlockManager blockManager)
{
    private readonly BlockManager blockManager = blockManager;
    // tolerance
    private const float t = 0.001f;

    public bool IsCollision(Collider c) => IsCollision(c.Position, c.Size);

    public bool IsCollision(Vector2 pos, Vector2 size) => GetCollisionCoords(pos, size).Count > 0;

    public HashSet<(int, int)> GetCollisionCoords(Vector2 pos, Vector2 size)
    {
        HashSet<(int, int)> output = [];
        int leftIndex = Funcs.GridCoord(pos.X + t);
        int rightIndex = Funcs.GridCoord(pos.X + size.X - t);
        int topIndex = Funcs.GridCoord(pos.Y + t);
        int bottomIndex = Funcs.GridCoord(pos.Y + size.Y - t);

        for (int y = topIndex; y <= bottomIndex; y++)
            for (int x = leftIndex; x <= rightIndex; x++)
                if (blockManager.HasBlockAt((x, y))) output.Add((x, y));

        return output;
    }

    public ((int, int)? collisionCoords, bool isGrounded, SurfaceType surface) ManageBlockCollision(
        Collider collider, Vector2 deltaPos)
    {
        Vector2 newPos = collider.Position + deltaPos;

        if (!IsCollision(newPos, collider.Size))
        {
            collider.SetPosition(newPos);
            return (null, false, SurfaceType.Normal);
        }

        var candidates = BuildCandidateSpots(newPos, collider.Size);
        var (freePos, offsets) = FindFreePosition(candidates, collider.Size);

        ApplyCollisionResponse(collider, freePos, offsets);

        bool groundHit = offsets.Any(o => o.Y < 0);
        SurfaceType surface = groundHit
            ? GetLandingSurface(freePos, collider.Size)
            : SurfaceType.Normal;

        return (Funcs.GridCoords(newPos), groundHit, surface);
    }

    // Builds the 8 candidate positions (4 orthogonal + 4 diagonal) around newPos,
    // each paired with the BFS expansion offsets for that direction.
    private static SortedDictionary<Vector2, List<Vector2>> BuildCandidateSpots(Vector2 newPos, Vector2 size)
    {
        float leftX = Funcs.ShoveTowardOrigin(newPos.X, size.X) - t;
        float rightX = Funcs.ShoveAwayOrigin(newPos.X) + t;
        float topY = Funcs.ShoveTowardOrigin(newPos.Y, size.Y) - t;
        float bottomY = Funcs.ShoveAwayOrigin(newPos.Y) + t;

        var spots = new SortedDictionary<Vector2, List<Vector2>>(new Funcs.VectorComparer<Vector2>(newPos));

        // Orthogonal — single expansion axis
        spots[new(newPos.X, topY)] = [new(0, -Consts.BlockWidth)];
        spots[new(newPos.X, bottomY)] = [new(0, Consts.BlockWidth)];
        spots[new(leftX, newPos.Y)] = [new(-Consts.BlockWidth, 0)];
        spots[new(rightX, newPos.Y)] = [new(Consts.BlockWidth, 0)];

        // Diagonal — two expansion axes; key collision resolution picks one
        spots[new(leftX, topY)] = [new(0, -Consts.BlockWidth), new(-Consts.BlockWidth, 0)];
        spots[new(rightX, topY)] = [new(0, -Consts.BlockWidth), new(Consts.BlockWidth, 0)];
        spots[new(leftX, bottomY)] = [new(0, Consts.BlockWidth), new(-Consts.BlockWidth, 0)];
        spots[new(rightX, bottomY)] = [new(0, Consts.BlockWidth), new(Consts.BlockWidth, 0)];

        return spots;
    }

    // BFS over candidate spots (sorted by euclidean distance from origin) until a
    // collision-free position is found. Returns the free position and the offsets
    // that led there, which encode which axes were blocked.
    private (Vector2 freePos, List<Vector2> offsets) FindFreePosition(
        SortedDictionary<Vector2, List<Vector2>> candidates, Vector2 size)
    {
        while (true)
        {
            (var pos, var offsets) = candidates.First();
            candidates.Remove(pos);

            if (IsCollision(pos, size))
            {
                foreach (var offset in offsets)
                    candidates[pos + offset] = offsets;
            }
            else
            {
                return (pos, offsets);
            }
        }
    }

    private static void ApplyCollisionResponse(Collider collider, Vector2 freePos, List<Vector2> offsets)
    {
        if (offsets.Any(o => o.X == 0)) collider.SetVelocityY(0);
        if (offsets.Any(o => o.Y == 0)) collider.SetVelocityX(0);
        collider.SetPosition(freePos);
    }

    private SurfaceType GetLandingSurface(Vector2 landingPos, Vector2 size)
    {
        (int gx, int gy) = Funcs.GridCoords(landingPos + new Vector2(size.X / 2f, size.Y + t));
        if (blockManager.HasBlockAt((gx, gy)))
            return BlockManager.GetSurfaceType(blockManager.BlockAt((gx, gy)).Type);
        return SurfaceType.Normal;
    }
}