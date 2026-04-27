using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Sprint2.Entities.Plants;

namespace Sprint2.Util;

public static class Funcs
{

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

    /// <summary>d is the dimension of the hitbox in that direction</summary>
    public static float ShoveTowardOrigin(float pos, float d) {
        return GridCoord(pos+d) * Consts.BlockWidth - d;
    }
    
    public static float ShoveAwayOrigin(float pos) {
        return (GridCoord(pos) + 1) * Consts.BlockWidth;
    }

    public class VectorComparer<T>(Vector2 origin): IComparer<Vector2> {
        public int Compare(Vector2 p, Vector2 q) => (p-origin).LengthSquared().CompareTo((q-origin).LengthSquared());
    }

    //random
    private static Random random = new();
    public static float Random() => random.NextSingle();
    /// <returns>1 or -1 with 50/50 prob</returns>
    public static int PlusMinus() => random.Next(2) == 0 ? -1 : 1;
    public static int RandInt(int max) => random.Next(max);
    public static int RandInt(int min, int max) => random.Next(min, max);

    public static IEnumerable<T> ListShuffle<T>(IEnumerable<T> list)
    {
        foreach (int i in RandRange(list.Count()))
        {
            yield return list.ElementAt(i);
        }
    }

    /// <summary>DO NOT USE FOR LARGE N</summary>
    public static IEnumerable<int> RandRange(int n) {
        return Enumerable.Range(0, n).OrderBy(x => random.Next());
    }


    //Pitch shifting
    public static float SpeciesScale(Species species) => species switch
    {
        Species.Grass => RandMajor7Add9Add13(),
        Species.Pineapple => RandPentatonic(),
        Species.Sandbox => RandMajor7Add9Add13(),
        Species.Apple => RandMajor7Add9Add13(),
        _ => RandDim7(),
    };

    public static float RandPentatonic() => (new int[] {0, -2, -4, -7, -9})[RandInt(5)]/12f;
    public static float RandMajor7Add9Add13() => ((new int[] {0, 4, 7, 11, 14, 18})[RandInt(5)]-9)/12f;
    public static float RandDim7() => ((new int[] {0, 3, 6, 10, 15})[RandInt(5)]-7)/12f;

    public static void MuteAndUnmuteMusic()
    {
        if (MediaPlayer.State == MediaState.Paused) MediaPlayer.Resume();
        else MediaPlayer.Pause();
    }
    
}