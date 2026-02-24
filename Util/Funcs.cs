using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprint2.Util;

public static class Funcs
{
    public static IEnumerable<T> ListShuffle<T>(IEnumerable<T> list)
    {
        foreach (int i in RandRange(list.Count()))
        {
            yield return list.ElementAt(i);
        }
    }

    //DO NOT USE FOR LARGE N
    public static IEnumerable<int> RandRange(int n)
    {
        return Enumerable.Range(0, n).OrderBy(x => Random.Shared.Next());
    }
}