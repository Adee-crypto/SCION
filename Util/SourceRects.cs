using Microsoft.Xna.Framework;
using Sprint2.Entities;
using Sprint2.Entities.Plants;
using System.Collections.Generic;

namespace Sprint2.Util;

public static class SourceRects
{
    private static Rectangle TileSourceRectAt(int x, int y)
    {
        return new(x * Consts.cellWidth, y * Consts.cellWidth, Consts.cellWidth, Consts.cellWidth);
    }

    public static Dictionary<State, Rectangle[]> PlayerSourceRects = new() {
        { State.LeftFacing, [TileSourceRectAt(2, 0)] },
        { State.LeftRunning, [TileSourceRectAt(2, 0), TileSourceRectAt(3, 0)] },
        { State.RightFacing, [TileSourceRectAt(0, 0)] },
        { State.RightRunning, [TileSourceRectAt(0, 0), TileSourceRectAt(1, 0)] },
        { State.LeftAttack, [TileSourceRectAt(1, 2)]},
        { State.RightAttack, [TileSourceRectAt(0, 2)]},
        { State.LeftFalling, [TileSourceRectAt(3, 0)] },
        { State.RightFalling, [TileSourceRectAt(1, 0)] },
        { State.BlockBreaking, [TileSourceRectAt(1, 5)] },
        { State.Dead, [TileSourceRectAt(0, 5)] }
    };

    public static Dictionary<State, Rectangle[]> EnemySourceRects = new()
    {
        { State.LeftFacing, [TileSourceRectAt(2, 0)] },
        { State.LeftRunning, [TileSourceRectAt(2, 0), TileSourceRectAt(3, 0)] },
        { State.RightFacing, [TileSourceRectAt(0, 0)] },
        { State.RightRunning, [TileSourceRectAt(0, 0), TileSourceRectAt(1, 0)] },
        { State.LeftAttack, [TileSourceRectAt(2,0)]},
        { State.RightAttack, [TileSourceRectAt(0, 0)]},
        { State.Dead, [TileSourceRectAt(2, 5)] }
    };

    public static Dictionary<Species, Rectangle> SpeciesSourceRects = new()
    {
        { Species.grass, TileSourceRectAt(2, 9) },
        { Species.apple, TileSourceRectAt(2, 10) },
        { Species.pineapple, TileSourceRectAt(2, 11) },
    };

    public static Dictionary<Species, Rectangle[]> SeedSourceRects = new()
    {
        { Species.grass, [TileSourceRectAt(6, 16), TileSourceRectAt(7, 16)] },
        { Species.apple, [TileSourceRectAt(9, 16), TileSourceRectAt(8, 16)] },
        { Species.pineapple, [TileSourceRectAt(2, 16), TileSourceRectAt(4, 16)] },
    };

    public static Dictionary<Platform.Type, Rectangle> PlatformSourceRects = new()
    {
        { Platform.Type.stone, TileSourceRectAt(1, 0) },
        { Platform.Type.stonebrick, TileSourceRectAt(6, 3) },
        { Platform.Type.crackedstonebrick, TileSourceRectAt(5, 6) },
    };

    public static Dictionary<string, Rectangle[]> ProjectileSourceRects = new()
    {
        { "grass seed", [TileSourceRectAt(6, 16), TileSourceRectAt(7, 16)] },
        { "apple seed", [TileSourceRectAt(9, 16), TileSourceRectAt(8, 16)] },
        { "pineapple seed", [TileSourceRectAt(2, 16), TileSourceRectAt(4, 16)] },
        { "VoidShot", [TileSourceRectAt(2, 16), TileSourceRectAt(4, 16)] }
    };
}