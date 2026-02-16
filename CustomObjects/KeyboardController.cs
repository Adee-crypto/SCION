using Microsoft.Xna.Framework.Input;
using System;
using Interfaces;
using System.Security.Cryptography;


namespace Sprint2.Controllers
{
    public class KeyBoardController : IController
    {
        private KeyboardState previousKeyboardState = new KeyboardState();

        public void Update()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            foreach ((Keys[] keySet, Action command) in CommandUtil.keyCommandBindings)
            {
                bool pausePressed = currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape);

                if (keySet.Length == 1 && keySet[0] == Keys.Escape)
                {
                    if (pausePressed) command.Execute(0);
                    continue;
                }

                bool currentKeyPressed = false;
                bool previousKeyPressed = false;

                foreach (Keys key in keySet)
                {
                    if (currentKeyboardState.IsKeyDown(key))
                    {
                        currentKeyPressed = true;
                        command();
                    }
                    else if (previousKeyboardState.IsKeyDown(key))
                    {
                        previousKeyPressed = true;
                    }
                }
            }

            previousKeyboardState = currentKeyboardState;
        }
    }
}
