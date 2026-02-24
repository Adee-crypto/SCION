using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Sprint2.UI;

public static class KeyBindings
{
    public static Dictionary<Keys[], Action> holdKeyBindings;
    public static Dictionary<Keys[], Action> tapKeyBindings;
    
    public static void AttachKeyBindings(Game1 game) {
        holdKeyBindings = new()
        {
            {new[] {Keys.Left, Keys.A}, () => game.player0.Move(-1)},
            {new[] {Keys.Right, Keys.D}, () => game.player0.Move(1)},
            {new[] {Keys.Up, Keys.W}, game.player0.Jump},
            {new[] {Keys.Z, Keys.N}, game.player0.Attack},
            {new[] {Keys.Down, Keys.S}, game.player0.BreakBlock}
        };
        tapKeyBindings = new()
        {
            {new[] {Keys.Escape}, game.TogglePause},
            {new[] {Keys.Q}, game.Exit},
            {new[] {Keys.R}, game.ResetLevel},
            {new[] {Keys.D2}, game.testPlant.ToggleSpecies}, //for testing
            {new[] {Keys.E}, game.player0.Damaged}
        };
    }
}