using Sprint2.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint2.UI;
using System;


namespace Sprint2.Controllers;

public static class KeyboardController// : IController
{
    private static KeyboardState previousKeyboardState;
    public static bool IsPaused { get; set; }

    public static void Update()
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();

        //Commands to execute while key held
        if (!IsPaused)
        {
            foreach ((Keys[] keySet, Action command) in KeyBindings.HoldKeyBindings)
            {
                foreach (Keys key in keySet)
                {
                    if (currentKeyboardState.IsKeyDown(key))
                    {
                        command();
                    }
                }
            }
        }

        //Comands to execute on key press
        foreach ((Keys[] keySet, Action command) in KeyBindings.TapKeyBindings)
        {
            foreach (Keys key in keySet)
            {
                if (currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
                {
                    if (!IsPaused && key == Keys.Q) return; //prevent quitting when not paused
                    command();
                }
            }
        }

        previousKeyboardState = currentKeyboardState;
    }
}