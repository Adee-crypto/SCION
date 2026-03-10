using Microsoft.Xna.Framework.Input;
using Sprint2.Extensions;
using System;
using System.Collections.Generic;

namespace Sprint2.UI;

public static class KeyBindings
{
    public static Dictionary<Keys[], Action> HoldKeyBindings { get; private set; }
    public static Dictionary<Keys[], Action> TapKeyBindings { get; private set; }

    private static IPlayer Target(Game1 game) => (game.ScreenManager.Current as IPlayerProvider)?.CurrentPlayer;

    public static void AttachKeyBindings(Game1 game)
    {
        HoldKeyBindings = new()
        {
            {new[] {Keys.Left, Keys.A}, () => Target(game)?.Move(-1)},
            {new[] {Keys.Right, Keys.D}, () => Target(game)?.Move(1)},
            {new[] {Keys.Up, Keys.W}, () => Target(game)?.Jump()},
            {new[] {Keys.Z, Keys.N}, () => Target(game)?.Attack()},
            {new[] {Keys.Down, Keys.S}, () => Target(game)?.BreakBlock()}
        };
        TapKeyBindings = new()
        {
            {new[] {Keys.Escape}, game.TogglePause},
            {new[] {Keys.Q}, game.Exit},
            {new[] {Keys.R}, game.ResetLevel},
            {new[] {Keys.E}, () => Target(game)?.ToggleDamaged()} //also for testing
        };
    }
}