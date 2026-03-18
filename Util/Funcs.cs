using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Sprint2.Util;

public static class Funcs
{
    private static readonly Random random = new();

    public static int RandInt(int max) => random.Next(max);
    public static int RandInt(int min, int max) => random.Next(min, max);

    //Converts pixel coords to grid (block) coords
    /// <param name="pos"></param>
    /// <returns>((int) (pos.X/Consts.BlockWidth), (int) (pos.Y/Consts.BlockWidth))</returns>
    public static (int, int) GridCoords(Vector2 pos) {
        return ((int) (pos.X/Consts.BlockWidth), (int) (pos.Y/Consts.BlockWidth));
    }

    //Converts pixel coord to grid (block) coord
    /// <param name="x"></param>
    /// <returns>(int) (x/Consts.BlockWidth)</returns>
    public static int GridCoord(float x) {
        return (int) (x/Consts.BlockWidth);
    }

    public static IEnumerable<T> ListShuffle<T>(IEnumerable<T> list)
    {
        foreach (int i in RandRange(list.Count()))
        {
            yield return list.ElementAt(i);
        }
    }

    /// <summary>DO NOT USE FOR LARGE N</summary>
    public static IEnumerable<int> RandRange(int n)
    {
        return Enumerable.Range(0, n).OrderBy(x => Random.Shared.Next());
    }

    public class VectorComparer<T>(Vector2 origin): IComparer<Vector2> {
        public int Compare(Vector2 p, Vector2 q) => (p-origin).LengthSquared().CompareTo((q-origin).LengthSquared());
    }
}