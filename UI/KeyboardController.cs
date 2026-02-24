using Interfaces;
using Microsoft.Xna.Framework.Input;
using Sprint2.UI;
using System;


namespace Sprint2.Controllers;

public class KeyBoardController : IController
{
    private KeyboardState previousKeyboardState = new KeyboardState();
    public bool IsPaused { get; set; } = false;

    public void Update()
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();

        //Commands to execute while key held
        if (!IsPaused)
        {
            foreach ((Keys[] keySet, Action command) in KeyBindings.holdKeyBindings)
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
        foreach ((Keys[] keySet, Action command) in KeyBindings.tapKeyBindings)
        {
            foreach (Keys key in keySet)
            {
                if (currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
                {
                    command();
                }
            }
        }

        previousKeyboardState = currentKeyboardState;
    }
}