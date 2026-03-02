using Microsoft.Xna.Framework;
using Sprint2.Util;
using Sprint2.Extensions;

namespace Sprint2.Entities.Projectiles;

public class ProjectileDef : Animated {
    public string ProjectileID { get; }
    public float MaxLifetimeSeconds { get; }
    public float LaunchSpeed { get; }
    public float Gravity { get; }
    public Vector2 Origin => new Vector2(CurrentSourceRect.Width, CurrentSourceRect.Height) / 2f;
    
    public ProjectileDef (string projectileID, float projectileMaxTime = 5f, float projectileLaunchSpeed = 200f, float projectileGravity = 98f) {
        ProjectileID = projectileID;
        MaxLifetimeSeconds = projectileMaxTime;
        LaunchSpeed = projectileLaunchSpeed;
        Gravity = projectileGravity;
        ResetFrameState(SourceRects.ProjectileSourceRects[ProjectileID]);
    }
}