using Microsoft.Xna.Framework;

namespace Sprint2.Util;

public static class Tunables
{
    //physics
    public static TunableFloat DefaultGravity {get;} = new(Consts.defaultGravity);
    public static TunableFloat DefaultProjectileGravity {get;} = new(Consts.defaultProjectileGravity);
    public static TunableFloat VoidProjectileGravity {get;} = new(Consts.voidProjectileGravity);
    public static TunableFloat AirResistance {get;} = new(Consts.AirResistance);
    public static TunableFloat GroundResistance {get;} = new(Consts.GroundResistance);

    public static TunableFloat PlayerGroundXForce {get;} = new(Consts.playerGroundXForce);
    public static TunableFloat PlayerAirXForce {get;} = new(Consts.playerAirXForce);
    public static TunableFloat PlayerTargetWalkVelocity {get;} = new(Consts.playerTargetWalkVelocity);
    public static TunableFloat PlayerYForce {get;} = new(Consts.playerYForce);
    public static TunableFloat PlayerMass {get;} = new(Consts.playerMass);
    
    public static TunableFloat EnemyXForce {get;} = new(Consts.enemyXForce);

    public static TunableFloat ProjectileMass {get;} = new(Consts.projectileMass);
    
    //timing
    public static TunableFloat BreakDuration {get;} = new(Consts.breakDuration);
    public static TunableFloat PlayerFrameTime {get;} = new(Consts.playerFrameTime);

    public static TunableFloat EnemyAttackCoolDown {get;} = new(Consts.enemyAttackCoolDown);
    public static TunableFloat EnemyAttackDuration {get;} = new(Consts.enemyAttackDuration);

    //distances
    public static TunableFloat EnemyPatrolDistance {get;} = new(Consts.enemyPatrolDistance);
    public static TunableFloat EnemyAttackDistance {get;} = new(Consts.enemyAttackDistance);
    public static TunableFloat EnemyRangeAttackDistance {get;} = new(Consts.enemyRangeAttackDistance);
}