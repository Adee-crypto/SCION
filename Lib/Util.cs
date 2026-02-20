using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Sprint2.Sprites.PlayerSprite;
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
            {new[] {Keys.Left, Keys.A}, () => game.player0.MoveLeft()},
            {new[] {Keys.Right, Keys.D}, () => game.player0.MoveRight()},
            {new[] {Keys.Up, Keys.W}, () => game.player0.Jump()},
            {new[] {Keys.Z, Keys.N}, () => game.player0.Attack()},
            {new[] {Keys.Down, Keys.S}, () => game.player0.BreakBlock()}
        };
        tapKeyCommandBindings = new()
        {
            {new[] {Keys.Escape}, () => game.TogglePause()},
            {new[] {Keys.Q}, () => game.Exit()},
            {new[] {Keys.R}, () => game.ResetLevel()},
            {new[] {Keys.D2}, () => game.testPlant.ToggleSpecies()} //for testing
        };
    }
}

public static class PlayerUtil
{
    public enum PlayerAction
    {
        None,
        Attack,
        PlantSeed,
        BreakBlock 
    };

    public enum PlayerAnimation
    {
        LeftFacing,
        LeftRunning,
        RightFacing,
        RightRunning,
        LeftAttack,
        RightAttack,
        LeftFalling,
        RightFalling,
        BlockBreaking
    };
    public const float breakDuration = 1f;
    public const int hitboxSize = 16;
    public const float secondsPerFrame = 0.2f;
    public const float horizontalSpeed = 150f;
    public const float jumpSpeed = -450f;
    public const float gravity = 980f;

    public static Texture2D playerTexture; // Set in Game1.LoadContent
    public static Texture2D arrowTexture; // Set in Game1.LoadContent

    public static Dictionary<PlayerAnimation, Rectangle[]> GetFrames()
    {
        Dictionary<PlayerAnimation, Rectangle[]> framesMap = new()
        {
            { PlayerAnimation.LeftFacing, [new (32, 0, 16, 16)] },
            { PlayerAnimation.LeftRunning, [new(32, 0, 16, 16), new(48, 0, 16, 16)] },
            { PlayerAnimation.RightFacing, [new (0, 0, 16, 16)] },
            { PlayerAnimation.RightRunning, [new(0, 0, 16, 16), new(16, 0, 16, 16)] },
            { PlayerAnimation.LeftAttack, [new(16, 32, 16, 16)]},
            { PlayerAnimation.RightAttack, [new(0, 32, 16, 16)]},
            { PlayerAnimation.LeftFalling, [new(48, 0, 16, 16)] },
            { PlayerAnimation.RightFalling, [new(16, 0, 16, 16)] },
            { PlayerAnimation.BlockBreaking, [new(16, 80, 16, 16)] }
        };
        return framesMap;
    }
}

public static class PlantUtil 
{
    public const int cellWidth = 16; 
    public static Texture2D spritesheet; // Set in Game1.LoadContent
    public static List<(int, int)> growDirs = [(0, 1), (0, -1), (1, 0), (-1, 0)];

    public static Dictionary<Plant.Species, Rectangle> SpeciesSpriteRects = new()
    {
        { Plant.Species.grass, new (2*cellWidth, 9*cellWidth, cellWidth, cellWidth) },
        { Plant.Species.apple, new (2*cellWidth, 10*cellWidth, cellWidth, cellWidth) },
        { Plant.Species.pineapple, new (2*cellWidth, 11*cellWidth, cellWidth, cellWidth) },
    };

    public static Dictionary<Plant.Species, float> SpeciesGrowTimes = new()
    {
        { Plant.Species.grass, 0.2f },
        { Plant.Species.apple, 0.5f },
        { Plant.Species.pineapple, 1.0f },
    };

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

public static class PlatformUtil 
{
    public const int platformWidth = 16;
    public static Texture2D spritesheet; // Set in Game1.LoadContent

    public static Dictionary<Platform.Type, Rectangle> PlatformSpriteRects = new()
    {
        { Platform.Type.stone, new (platformWidth, 0, platformWidth, platformWidth) },
        { Platform.Type.stonebrick, new (6*platformWidth, 3*platformWidth, platformWidth, platformWidth) },
        { Platform.Type.cracked_stonebrick, new (5*platformWidth, 6*platformWidth, platformWidth, platformWidth) },
    };
}

public static class UIUtil {
    public static Texture2D buttonTexture; // Set in Game1.LoadContent
    public static Texture2D resetTexture; // Set in Game1.LoadContent
    public static SpriteFont uiFont; // Set in Game1.LoadContent
}

public static class ScreenUtil {
    public static (int w, int h) defaultScreenSize = (1000, 800);
}