using System;
using System.Collections.Generic;
using Sprint2.Entities.Plants;
using Sprint2.Managers;

namespace Sprint2.Entities.Projectiles;

public static class ProjectileUtil
{
    public static Dictionary<ProjectileType, Func<BlockManager, (int, int), Plant>> ProjectileToPlant { get; } = new() {
        { ProjectileType.Grass, (c, r) => new GrassPlant(c, r) },
        { ProjectileType.Apple, (c, r) => new ApplePlant(c, r) },
        { ProjectileType.Pineapple, (c, r) => new PineapplePlant(c, r) },
        { ProjectileType.Sandbox, (c, r) => new SandboxPlant(c, r) },
        { ProjectileType.Gravebind, (c, r) => new GravebindRootPlant(c, r) },
        // Cherry and Catalyst not throwable
    };
}