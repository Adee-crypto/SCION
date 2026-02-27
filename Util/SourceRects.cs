using Microsoft.Xna.Framework;
using Sprint2.Entities.Plants;
using Players = Sprint2.Entities.Players;
using Enemies = Sprint2.Entities.Enemies;
using System.Collections.Generic;
using Sprint2.Entities;

namespace Sprint2.Util;

public static class SourceRects
{
    private static Rectangle TileSourceRectAt(int x, int y)
    {
        return new(x * Consts.cellWidth, y * Consts.cellWidth, Consts.cellWidth, Consts.cellWidth);
    }

    public static Dictionary<Players.SpriteState, Rectangle[]> PlayerSourceRects {get;} = new() {
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

    public static Dictionary<Enemies.SpriteState, Rectangle[]> EnemySourceRects {get;} = new()
    {
        { Enemies.SpriteState.LeftFacing, [TileSourceRectAt(2, 0)] },
        { Enemies.SpriteState.LeftRunning, [TileSourceRectAt(2, 0), TileSourceRectAt(3, 0)] },
        { Enemies.SpriteState.RightFacing, [TileSourceRectAt(0, 0)] },
        { Enemies.SpriteState.RightRunning, [TileSourceRectAt(0, 0), TileSourceRectAt(1, 0)] },
        { Enemies.SpriteState.LeftAttack, [TileSourceRectAt(2,0)]},
        { Enemies.SpriteState.RightAttack, [TileSourceRectAt(0, 0)]},
        { Enemies.SpriteState.Dead, [TileSourceRectAt(2, 5)] }
    };

    public static Dictionary<Species, Rectangle> SpeciesSourceRects {get;} = new()
    {
        { Species.grass, TileSourceRectAt(2, 9) },
        { Species.apple, TileSourceRectAt(2, 10) },
        { Species.pineapple, TileSourceRectAt(2, 11) },
    };

    public static Dictionary<Species, Rectangle[]> SeedSourceRects {get;} = new()
    {
        { Species.grass, [TileSourceRectAt(6, 16), TileSourceRectAt(7, 16)] },
        { Species.apple, [TileSourceRectAt(9, 16), TileSourceRectAt(8, 16)] },
        { Species.pineapple, [TileSourceRectAt(2, 16), TileSourceRectAt(4, 16)] },
    };

    public static Dictionary<Platform.Type, Rectangle> PlatformSourceRects {get;} = new()
    {
        { Platform.Type.stone, TileSourceRectAt(1, 0) },
        { Platform.Type.stonebrick, TileSourceRectAt(6, 3) },
        { Platform.Type.crackedstonebrick, TileSourceRectAt(5, 6) },
    };

    public static Dictionary<string, Rectangle[]> ProjectileSourceRects {get;} = new()
    {
        { "grass seed", [TileSourceRectAt(6, 16), TileSourceRectAt(7, 16)] },
        { "apple seed", [TileSourceRectAt(9, 16), TileSourceRectAt(8, 16)] },
        { "pineapple seed", [TileSourceRectAt(2, 16), TileSourceRectAt(4, 16)] },
        { "VoidShot", [TileSourceRectAt(2, 16), TileSourceRectAt(4, 16)] }
    };
}