using System.Collections.Generic;

namespace Sprint2.Entities.Plants;

public static class PlantUtil
{
    public static List<(int, int)> growDirs = [(0, 1), (0, -1), (1, 0), (-1, 0)];

    public static Dictionary<Species, float> SpeciesGrowTimes = new()
    {
        {Species.grass, 0.1f },
        {Species.apple, 0.2f },
        {Species.pineapple, 0.4f },
    };
}