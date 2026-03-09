using Microsoft.Xna.Framework;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.Entities.Projectiles;

public class ProjectileSprite : Animated
{
    public float MaxLifetimeSeconds { get; }
    public Vector2 Origin => new Vector2(CurrentSourceRect.Width, CurrentSourceRect.Height) / 2f;

    public ProjectileSprite(ProjectileType projectileID, float projectileMaxTime)
    {
        MaxLifetimeSeconds = projectileMaxTime;
        ResetFrameState(SourceRects.ProjectileSourceRects[projectileID]);
    }
}