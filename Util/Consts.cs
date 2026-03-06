namespace Sprint2.Util;

public static class Consts
{
    public static (int w, int h) DefaultScreenSize { get; }= (1000, 800);

    //player
    public const float breakDuration = 1f;
    public const int playerHitboxSize = 16;
    public const float playerFrameTime = 0.2f;
    public const float playerXSpeed = 150f;
    public const float playerJumpSpeed = -450f;
    public const float playerGravity = 980f;

    //Blocks
    public const int BlockWidth = 16;
}