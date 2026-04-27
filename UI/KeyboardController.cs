using Microsoft.Xna.Framework.Input;
using Sprint2.UI;
using System;
using System.Linq;


namespace Sprint2.Controllers;

public static class KeyboardController// : IController
{
    private static KeyboardState previousKeyboardState;

    public static void Update(bool isPaused)
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();

        //Commands to execute while key held
        if (!isPaused) ExecuteHoldBindings(currentKeyboardState);

        //Comands to execute on key press
        ExecuteTapBindings(currentKeyboardState, isPaused);

        previousKeyboardState = currentKeyboardState;
    }

    private static void ExecuteHoldBindings(KeyboardState currentKeyboardState)
    {
        foreach (var (keySet, command) in KeyBindings.HoldKeyBindings)
        {
            if (!keySet.Any(currentKeyboardState.IsKeyDown)) continue;

            command();
        }
    }

    private static void ExecuteTapBindings(KeyboardState currentKeyboardState, bool isPaused)
    {
        Keys[] tappedKeys = [.. currentKeyboardState.GetPressedKeys().Where(previousKeyboardState.IsKeyUp)];
        
        foreach (var (keySet, command) in KeyBindings.TapKeyBindings)
        {
            if (!keySet.Any(tappedKeys.Contains)) continue;
            if (!isPaused && keySet.Contains(Keys.Q)) continue;

            command();
        }
    }
}