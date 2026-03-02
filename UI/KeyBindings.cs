using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Sprint2.UI;

public static class KeyBindings
{
    public static Dictionary<Keys[], Action> HoldKeyBindings {get; private set;}
    public static Dictionary<Keys[], Action> TapKeyBindings {get; private set;}

    public static void AttachKeyBindings(Game1 game)
    {
        HoldKeyBindings = new()
        {
            {new[] {Keys.Left, Keys.A}, () => game.Player.Move(-1)},
            {new[] {Keys.Right, Keys.D}, () => game.Player.Move(1)},
            {new[] {Keys.Up, Keys.W}, () => game.Player.Jump()},
            {new[] {Keys.Z, Keys.N}, () => game.Player.Attack()},
            {new[] {Keys.Down, Keys.S}, () => game.Player.BreakBlock()}
        };
        TapKeyBindings = new()
        {
            {new[] {Keys.Escape}, game.TogglePause},
            {new[] {Keys.Q}, game.Exit},
            {new[] {Keys.R}, game.ResetLevel},
            {new[] {Keys.E}, game.Player.ToggleDamaged} //also for testing
        };
    }
}