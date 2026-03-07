using Microsoft.Xna.Framework;
using Sprint2.Entities.Plants;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.Entities.Projectiles;

public enum ProjectileType
{
    Void,
    Grass,
    Apple,
    Pineapple,
}

public class ProjectileSprite : Animated
{
    public static ProjectileType SpeciesToProjectileType(Species s) => s switch
    {
        Species.grass => ProjectileType.Grass,
        Species.apple => ProjectileType.Apple,
        Species.pineapple => ProjectileType.Pineapple,
        _ => throw new System.NotImplementedException(),
    };

    public ProjectileType ProjectileID { get; }
    public float MaxLifetimeSeconds { get; }
    public Vector2 Origin => new Vector2(CurrentSourceRect.Width, CurrentSourceRect.Height) / 2f;

    public ProjectileSprite(ProjectileType projectileID, float projectileMaxTime)
    {
        ProjectileID = projectileID;
        MaxLifetimeSeconds = projectileMaxTime;
        ResetFrameState(SourceRects.ProjectileSourceRects[ProjectileID]);
    }
}