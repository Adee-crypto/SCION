using Microsoft.Xna.Framework;
using Sprint2.Util;

namespace Sprint2.Entities.Projectiles;

public class ProjectileDef : Animated {
    public string ProjectileID { get; }
    public float MaxLifetimeSeconds { get; }
    public float LaunchSpeed { get; }
    public float Gravity { get; }

    //Assumes width of all frames is the same, and height is the same. Also assumes hitbox is the same as the frame size
    public Vector2 Origin => CurrentSourceRect.Size.ToVector2() / 2f;
    public Vector2 HitBox => CurrentSourceRect.Location.ToVector2();
    
    public ProjectileDef (string projectileID, float projectileMaxTime = 5f, float projectileLaunchSpeed = 200f, float projectileGravity = 98f) {
        ProjectileID = projectileID;
        MaxLifetimeSeconds = projectileMaxTime;
        LaunchSpeed = projectileLaunchSpeed;
        Gravity = projectileGravity;
        ResetFrameState(SourceRects.ProjectileSourceRects[ProjectileID]);
    }
}