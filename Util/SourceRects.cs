using Microsoft.Xna.Framework;
using Sprint2.Entities.Projectiles;
using static Sprint2.Managers.BlockManager.Block;
using System.Collections.Generic;
using Enemies = Sprint2.Entities.Enemies;
using Players = Sprint2.Entities.Players;

namespace Sprint2.Util;

public static class SourceRects
{
    //for now the testsheet.png has 16x16 tiles
    private static Rectangle TileAt(int x, int y) => new(x * 16, y * 16, 16, 16);

    public static Dictionary<Players.SpriteState, Rectangle[]> PlayerSourceRects { get; } = new() {
        { Players.SpriteState.LeftFacing, [TileAt(2, 0)] },
        { Players.SpriteState.LeftRunning, [TileAt(2, 0), TileAt(3, 0)] },
        { Players.SpriteState.RightFacing, [TileAt(0, 0)] },
        { Players.SpriteState.RightRunning, [TileAt(0, 0), TileAt(1, 0)] },
        { Players.SpriteState.LeftAttack, [TileAt(1, 2)]},
        { Players.SpriteState.RightAttack, [TileAt(0, 2)]},
        { Players.SpriteState.LeftFalling, [TileAt(3, 0)] },
        { Players.SpriteState.RightFalling, [TileAt(1, 0)] },
        { Players.SpriteState.BlockBreaking, [TileAt(1, 5)] },
        { Players.SpriteState.Dead, [TileAt(0, 5)] }
    };

    public static Dictionary<Enemies.SpriteState, Rectangle[]> EnemySourceRects { get; } = new()
    {
        { Enemies.SpriteState.LeftFacing, [TileAt(2, 0)] },
        { Enemies.SpriteState.LeftRunning, [TileAt(2, 0), TileAt(3, 0)] },
        { Enemies.SpriteState.RightFacing, [TileAt(0, 0)] },
        { Enemies.SpriteState.RightRunning, [TileAt(0, 0), TileAt(1, 0)] },
        { Enemies.SpriteState.LeftAttack, [TileAt(2,0)]},
        { Enemies.SpriteState.RightAttack, [TileAt(0, 0)]},
        { Enemies.SpriteState.Dead, [TileAt(2, 5)] }
    };

    public static Dictionary<BlockType, Rectangle> BlockSourceRects { get; } = new()
    {
        { BlockType.Grass, TileAt(2, 9) },
        { BlockType.Apple, TileAt(2, 10) },
        { BlockType.Pineapple, TileAt(2, 11) },
        { BlockType.Sandbox, TileAt(8, 0) },
        { BlockType.Dirt, TileAt(2, 0) },
        { BlockType.Stone, TileAt(1, 0) },
        { BlockType.StoneBrick, TileAt(6, 3) },
        { BlockType.CrackedStoneBrick, TileAt(5, 6) },
        { BlockType.Void, TileAt(7, 11) },
        { BlockType.Cherry, TileAt(3, 9) },
    };

    public static Dictionary<ProjectileType, Rectangle[]> ProjectileSourceRects { get; } = new()
    {
        { ProjectileType.Grass, [TileAt(6, 16), TileAt(7, 16)] },
        { ProjectileType.Apple, [TileAt(9, 16), TileAt(8, 16)] },
        { ProjectileType.Pineapple, [TileAt(2, 16), TileAt(4, 16)] },
        { ProjectileType.Sandbox, [TileAt(16, 2), TileAt(17, 2)] },
        { ProjectileType.Void, [TileAt(2, 16), TileAt(4, 16)] }
    };
}