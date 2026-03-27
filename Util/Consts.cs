using Microsoft.Xna.Framework;

namespace Sprint2.Util;

public static class Consts
{
    //player stats
    public const int defaultPlayerHealth = 5;

    //other
    public static readonly (int, int)[] orthoDirs = [(0, 1), (0, -1), (1, 0), (-1, 0)];
    
    //Screen dimensions
    public static (int w, int h) DefaultScreenSize { get; } = (1000, 800);
    public const int BlockWidth = 16;

    //physics
    public const float defaultGravity = 980f;
    public const float defaultProjectileGravity = 98f;
    public const float voidProjectileGravity = 0f;
    /// <summary> Fraction of original velocity, unitless (should probably change to depend on dt) </summary>
    public const float AirResistance = 0.96f;
    /// <summary> constant friction force, m/s^2 </summary>
    public const float GroundResistance = 600f;

    public const float playerGroundXForce = 60f;
    public const float playerAirXForce = 300f;
    public const float playerTargetWalkVelocity = 120f;
    public const float playerYForce = -24000f;
    public const float playerMass = 1f;
    public static Vector2 playerHitbox {get;} = new(16-0.1f, 16-0.1f); //fix this offset perhaps
    
    public const float enemyXForce = 100f;


    public const float projectileMass = 0.5f; // could be different using dictionary with projectileType
    
    //timing
    public const float breakDuration = 0.5f;
    public const float playerFrameTime = 0.2f;

    public const float enemyAttackCoolDown = 1f;
    public const float enemyAttackDuration = 0.1f;

    //distances
    public const float enemyPatrolDistance = 128f;
    public const float enemyAttackDistance = 16f;
    public const float enemyRangeAttackDistance = 64f;
}