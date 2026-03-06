using Microsoft.Xna.Framework;
using Sprint2.Util;
using Sprint2.Extensions;
using Sprint2.Entities.Plants;

namespace Sprint2.Entities.Projectiles;

public enum ProjectileType {
    Void,
    Grass,
    Apple,
    Pineapple,
}

public class ProjectileDef : Animated {
    public static ProjectileType SpeciesToProjectileType(Species s) => s switch {
        Species.grass => ProjectileType.Grass,
        Species.apple => ProjectileType.Apple,
        Species.pineapple => ProjectileType.Pineapple,
        _ => throw new System.NotImplementedException(),
    };

    public ProjectileType ProjectileID { get; }
    public float MaxLifetimeSeconds { get; }
    public float LaunchSpeed { get; }
    public float Gravity { get; }
    public Vector2 Origin => new Vector2(CurrentSourceRect.Width, CurrentSourceRect.Height) / 2f;
    
    public ProjectileDef (ProjectileType projectileID, float projectileMaxTime = 5f, float projectileLaunchSpeed = 200f, float projectileGravity = 98f) {
        ProjectileID = projectileID;
        MaxLifetimeSeconds = projectileMaxTime;
        LaunchSpeed = projectileLaunchSpeed;
        Gravity = projectileGravity;
        ResetFrameState(SourceRects.ProjectileSourceRects[ProjectileID]);
    }
}