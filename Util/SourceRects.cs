using Microsoft.Xna.Framework;
using Sprint2.Entities;
using Sprint2.Entities.Projectiles;
using System.Collections.Generic;
using Enemies = Sprint2.Entities.Enemies;
using Players = Sprint2.Entities.Players;

namespace Sprint2.Util;

public static class SourceRects
{
    private static Rectangle TileSourceRectAt(int x, int y)
    {
        return new(x * Consts.BlockWidth, y * Consts.BlockWidth, Consts.BlockWidth, Consts.BlockWidth);
    }

    public static Dictionary<Players.SpriteState, Rectangle[]> PlayerSourceRects { get; } = new() {
        { Players.SpriteState.LeftFacing, [TileSourceRectAt(2, 0)] },
        { Players.SpriteState.LeftRunning, [TileSourceRectAt(2, 0), TileSourceRectAt(3, 0)] },
        { Players.SpriteState.RightFacing, [TileSourceRectAt(0, 0)] },
        { Players.SpriteState.RightRunning, [TileSourceRectAt(0, 0), TileSourceRectAt(1, 0)] },
        { Players.SpriteState.LeftAttack, [TileSourceRectAt(1, 2)]},
        { Players.SpriteState.RightAttack, [TileSourceRectAt(0, 2)]},
        { Players.SpriteState.LeftFalling, [TileSourceRectAt(3, 0)] },
        { Players.SpriteState.RightFalling, [TileSourceRectAt(1, 0)] },
        { Players.SpriteState.BlockBreaking, [TileSourceRectAt(1, 5)] },
        { Players.SpriteState.Dead, [TileSourceRectAt(0, 5)] }
    };

    public static Dictionary<Enemies.SpriteState, Rectangle[]> EnemySourceRects { get; } = new()
    {
        { Enemies.SpriteState.LeftFacing, [TileSourceRectAt(2, 0)] },
        { Enemies.SpriteState.LeftRunning, [TileSourceRectAt(2, 0), TileSourceRectAt(3, 0)] },
        { Enemies.SpriteState.RightFacing, [TileSourceRectAt(0, 0)] },
        { Enemies.SpriteState.RightRunning, [TileSourceRectAt(0, 0), TileSourceRectAt(1, 0)] },
        { Enemies.SpriteState.LeftAttack, [TileSourceRectAt(2,0)]},
        { Enemies.SpriteState.RightAttack, [TileSourceRectAt(0, 0)]},
        { Enemies.SpriteState.Dead, [TileSourceRectAt(2, 5)] }
    };

    public static Dictionary<BlockType, Rectangle> BlockSourceRects { get; } = new()
    {
        { BlockType.Grass, TileSourceRectAt(2, 9) },
        { BlockType.Apple, TileSourceRectAt(2, 10) },
        { BlockType.Pineapple, TileSourceRectAt(2, 11) },
        { BlockType.Stone, TileSourceRectAt(1, 0) },
        { BlockType.StoneBrick, TileSourceRectAt(6, 3) },
        { BlockType.CrackedStoneBrick, TileSourceRectAt(5, 6) },
    };

    public static Dictionary<ProjectileType, Rectangle[]> ProjectileSourceRects { get; } = new()
    {
        { ProjectileType.Grass, [TileSourceRectAt(6, 16), TileSourceRectAt(7, 16)] },
        { ProjectileType.Apple, [TileSourceRectAt(9, 16), TileSourceRectAt(8, 16)] },
        { ProjectileType.Pineapple, [TileSourceRectAt(2, 16), TileSourceRectAt(4, 16)] },
        { ProjectileType.Void, [TileSourceRectAt(2, 16), TileSourceRectAt(4, 16)] }
    };
}