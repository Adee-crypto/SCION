using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Sprint2.UI;
using System;


namespace Sprint2.Controllers;

public class KeyBoardController : IController
{
    private KeyboardState previousKeyboardState;
    public bool IsPaused { get; set; }

    public void Update()
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