using Microsoft.Xna.Framework;

namespace Sprint2.Util;

public static class Consts
{
    //Screen dimensions
    public static (int w, int h) DefaultScreenSize { get; } = (1000, 800);
    public const int BlockWidth = 16;

    //physics
    public const float defaultGravity = 980f;
    public const float defaultProjectileGravity = 98f;
    public const float voidProjectileGravity = 0f;

    public const float playerXForce = 120f;
    public const float playerYForce = -450f;
    public const float playerMass = 1f;
    public static Vector2 playerHitbox {get;} = new(16-0.1f, 16-0.1f); //fix this offset perhaps
    
    public const float enemyXForce = 100f;

    public const float projectileMass = .2f; // could be different using dictionary with projectileType
    
    //timing
    public const float breakDuration = 0.5f;
    public const float playerFrameTime = 0.2f;

    public const float enemyAttackCoolDown = 0.5f;
    public const float enemyAttackDuration = 0.1f;

    //distances
    public const float enemyPatrolDistance = 128f;
    public const float enemyAttackDistance = 16f;
    public const float enemyRangeAttackDistance = 64f;
}