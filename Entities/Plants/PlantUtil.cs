using System.Collections.Generic;

namespace Sprint2.Entities.Plants;

public static class PlantUtil
{
    public static List<(int, int)> GrowDirs { get; } = [(0, 1), (0, -1), (1, 0), (-1, 0)];

    public static Dictionary<Species, float> SpeciesGrowTimes { get; } = new()
    {
        {Species.Grass, 0.1f },
        {Species.Apple, 0.2f },
        {Species.Pineapple, 0.4f },
        {Species.Sandbox, 0.2f }, //6 ticks until detonation
    };
}