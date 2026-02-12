using Microsoft.Xna.Framework.Input;
using Sprint2.Commands;
using Sprint2.Interfaces;


namespace Sprint2.Controllers
{
    public class KeyBoardController : IController
    {
        private CommandList commands;
        private KeyboardState previousKeyboardState;

        public KeyBoardController(Game1 game)
        {
            this.commands = new CommandList(game);
            previousKeyboardState = new KeyboardState();
        }

        public void Update()
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            foreach (Keys[] keySet in this.commands.KeyboardCommands.Keys)
            {
                bool currentKeyPressed = false;
                bool previousKeyPressed = false;

                bool upPressed = currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W);
                bool downPressed = currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S);
                bool leftPressed = currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A);
                bool rightPressed = currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D);

                bool horizontalCancel = leftPressed && rightPressed;
                bool verticalCancel = upPressed && downPressed;
                bool verticalPressed = upPressed || downPressed;

                bool moveCancel = horizontalCancel && !verticalPressed || verticalCancel; // press A or D when pressing W and S is crazy

                for (int i = 0; i < keySet.Length; i++)
                {
                    Keys key = keySet[i];
                    if (currentKeyboardState.IsKeyDown(key) && !moveCancel)
                    {
                        currentKeyPressed = true;
                        this.commands.KeyboardCommands[keySet].Execute(i);
                        break;
                    }
                    else if (previousKeyboardState.IsKeyDown(key))
                    {
                        previousKeyPressed = true;
                    }
                }

                if (previousKeyPressed && !currentKeyPressed) // stop pressing a key
                {
                    this.commands.KeyboardCommands[keySet].StopExecute();
                }
            }

            previousKeyboardState = currentKeyboardState;
        }
    }
}
