using Microsoft.Xna.Framework.Input;
using System;
using Interfaces;


namespace Sprint2.Controllers;

public class KeyBoardController : IController
{
    private KeyboardState previousKeyboardState = new KeyboardState();
    public bool IsPaused { get; set; }

    public void Update()
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();

        //Commands to execute while key held
        if (!IsPaused) {
            foreach ((Keys[] keySet, Action command) in CommandUtil.holdKeyCommandBindings) {
                foreach (Keys key in keySet){
                    if (currentKeyboardState.IsKeyDown(key)) {
                        command();
                    }
                }
            }
        }

        //Comands to execute on key press
        foreach ((Keys[] keySet, Action command) in CommandUtil.tapKeyCommandBindings) {
            foreach (Keys key in keySet) {
                if (currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key)) {
                    command();
                }
            }
        }

        previousKeyboardState = currentKeyboardState;
    }
}