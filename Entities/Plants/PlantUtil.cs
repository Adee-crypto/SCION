using System.Collections.Generic;

namespace Sprint2.Entities.Plants;

public static class PlantUtil
{
    public static List<(int, int)> GrowDirs { get; } = [(0, 1), (0, -1), (1, 0), (-1, 0)];

    public static Dictionary<Species, float> SpeciesGrowTimes { get; } = new()
    {
        {Species.grass, 0.1f },
        {Species.apple, 0.2f },
        {Species.pineapple, 0.4f },
        {Species.sandbox, 1f },
    };
}