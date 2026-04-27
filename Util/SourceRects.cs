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
    private static Rectangle TileAtPlayer(int x, int y) => new(x * 8 - 1 * x, y * 16, 8, 16);
    private static Rectangle TileAtPlayerJump(int x, int y) => new(x * 16 + 39, y * 16, 13, 16);
    private static Rectangle TileAt24(int x, int y) => new(x * 24, y * 24 + 33, 24, 24);

    public static Dictionary<Players.SpriteState, Rectangle[]> PlayerSourceRects { get; } = new() {
        { Players.SpriteState.LeftFacing, [TileAtPlayer(0, 0)] },
        { Players.SpriteState.LeftRunning, [TileAtPlayer(1, 0), TileAtPlayer(2, 0)] },
        { Players.SpriteState.RightFacing, [TileAtPlayer(0, 1)] },
        { Players.SpriteState.RightRunning, [TileAtPlayer(1, 1), TileAtPlayer(2, 1)] },
        { Players.SpriteState.LeftAttack, [TileAtPlayer(0, 0)] },
        { Players.SpriteState.RightAttack, [TileAtPlayer(0, 1)] },
        { Players.SpriteState.LeftFalling, [TileAtPlayerJump(0, 0)] },
        { Players.SpriteState.RightFalling, [TileAtPlayerJump(0, 1)] },
        { Players.SpriteState.BlockBreaking, [TileAtPlayer(0, 0)] },
        { Players.SpriteState.Dead, [TileAtPlayer(0, 0)] }
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
        { BlockType.Sandbox, TileAt24(0, 0) },
        { BlockType.Muck, TileAt24(1, 0) },
        { BlockType.Pineapple, TileAt24(3, 0) },
        { BlockType.Stone, TileAt24(0, 1) },
        { BlockType.StoneBrick, TileAt24(1, 1) },
        { BlockType.Apple, TileAt24(2, 1) },
        { BlockType.Grass, TileAt24(3, 1) },
        { BlockType.Snow, TileAt24(0, 2) },
        { BlockType.Dirt, TileAt24(2, 2) },
        { BlockType.Void, TileAt24(0, 3) },
        //stinky
        { BlockType.Cherry, TileAt24(3, 9) },
        { BlockType.Gravebind, TileAt24(4, 12) },  // Change to Appropriate blocktype
        { BlockType.Catalyst,  TileAt24(6, 12) },  // Change to Appropriate blocktype
        { BlockType.CrackedStoneBrick, TileAt24(5, 2) }, //fix this being duplicate, we prob dnot actually need 3 stone variants
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