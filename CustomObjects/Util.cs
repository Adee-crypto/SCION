using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Sprint2.Sprites.LinkSprite;
using System.Linq;
using System;

namespace Sprint2;

public static class CommandUtil
{
    public static Dictionary<Keys[], Action> holdKeyCommandBindings;
    public static Dictionary<Keys[], Action> tapKeyCommandBindings;
    
    public static void AttachCommandBindings(Game1 game) {
        holdKeyCommandBindings = new()
        {
            {[Keys.Left, Keys.A], game.Player.MoveLeft},
            {[Keys.Right, Keys.D], game.Player.MoveRight},
            {[Keys.Up, Keys.W], game.Player.Jump},
            {[Keys.Z, Keys.N], game.Player.Attack},
        };
        tapKeyCommandBindings = new()
        {
            {[Keys.P], game.TogglePause},
            {[Keys.Q], game.Exit}
        };
    }
}

public static class LinkUtil
{
    public const float secondsPerFrame = 0.005f;
    public const float horizontalSpeed = 100f;
    public const float jumpSpeed = -650f;
    public const float gravity = 980f;

    public static Texture2D texture; // Set in Game1.LoadContent

    public static Dictionary<LinkAnimationState, Rectangle[]> GetFrames()
    {
        Dictionary<LinkAnimationState, Rectangle[]> LinkFramesMap = new()
        {
            { LinkAnimationState.LeftFacing, [new (32, 0, 16, 16)] },
            { LinkAnimationState.LeftRunning, [new(32, 0, 16, 16), new(48, 0, 16, 16)] },
            { LinkAnimationState.RightFacing, [new (0, 0, 16, 16)] },
            { LinkAnimationState.RightRunning, [new(0, 0, 16, 16), new(16, 0, 16, 16)] },
            { LinkAnimationState.LeftAttack, [new(16, 32, 16, 16)]},
            { LinkAnimationState.RightAttack, [new(0, 32, 16, 16)]},
        };
        return LinkFramesMap;
    }
}

public static class PlantUtil {
    public const int cellWidth = 16; 

    public static Texture2D spritesheet; // Set in Game1.LoadContent

    public static Dictionary<Plant.Species, Rectangle> SpeciesSpriteRects = new()
    {
        { Plant.Species.grass, new (0, 0, cellWidth, cellWidth) },
        { Plant.Species.apple, new (cellWidth, 0, cellWidth, cellWidth) },
        { Plant.Species.pineapple, new (2*cellWidth, 0, cellWidth, cellWidth) },
    };

    public static List<(int, int)> growDirs = [(0, 1), (0, -1), (1, 0), (-1, 0)];

    public static IEnumerable<(int, int)> ShuffledDirs()
    {
        foreach (int i in RandRange(growDirs.Count))
        {
            yield return growDirs[i];
        }
    }

    //DO NOT USE FOR LARGE N
    public static IEnumerable<int> RandRange(int n)
    {
        return Enumerable.Range(0, n).OrderBy(x => Random.Shared.Next());
    }
}

public static class PlatformUtil {
    public const int platformWidth = 16; 

    public static Texture2D spritesheet; // Set in Game1.LoadContent

    public static Dictionary<Platform.Type, Rectangle> PlatformSpriteRects = new()
    {
        { Platform.Type.stone, new (platformWidth, 0, platformWidth, platformWidth) },
        { Platform.Type.stonebrick, new (6*platformWidth, 3*platformWidth, platformWidth, platformWidth) },
        { Platform.Type.cracked_stonebrick, new (5*platformWidth, 6*platformWidth, platformWidth, platformWidth) },
    };
}