using Microsoft.Xna.Framework;

namespace Sprint2.Util;

public static class Consts
{
    public static (int w, int h) DefaultScreenSize { get; } = (1000, 800);

    //player
    public const float breakDuration = 1f;
    public static Vector2 playerHitbox {get;} = new(16-0.1f, 16-0.1f); //fix this perhaps
    public const float playerFrameTime = 0.2f;
    public const float playerXSpeed = 120f;
    public const float playerJumpSpeed = -450f;
    public const float playerGravity = 980f;
    public const float playerMass = 1f;

    //projectiles
    public const float enemyProjectileGravity = 0f;
    public const float playerProjectileGravity = 98f;
    public const float projectileMass = 1f; // could be different using dictionary with projectileType

    //Blocks
    public const int BlockWidth = 16;
}