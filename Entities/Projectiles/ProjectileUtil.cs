using System;
using System.Collections.Generic;
using Sprint2.Entities.Plants;
using Sprint2.Entities.Projectiles;
using Sprint2.Levels;

namespace Sprint2.Entities.Projectiles;

public static class ProjectileUtil
{
    public static Dictionary<ProjectileType, Func<BaseLevel, (int, int), Plant>> ProjectileToPlant { get; } = new() {
        { ProjectileType.Grass, (c, r) => new GrassPlant(c, r) },
        { ProjectileType.Apple, (c, r) => new ApplePlant(c, r) },
        { ProjectileType.Pineapple, (c, r) => new PineapplePlant(c, r) },
        { ProjectileType.Sandbox, (c, r) => new SandboxPlant(c, r) },
    };
}