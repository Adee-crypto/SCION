using Microsoft.Xna.Framework.Input;
using Interfaces;
using System.Collections.Generic;


namespace Sprint2.Commands
{
    public class CommandList
    {
        private Game1 game;

        private Dictionary<Keys[], ICommand> keyboardCommandMap;

        public CommandList(Game1 game)
        {
            this.game = game;
            SetCommands();
        }

        private void SetCommands()
        {
            keyboardCommandMap = new Dictionary<Keys[], ICommand>()
            {
                {new Keys[] {Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.W, Keys.S, Keys.A, Keys.D}, new LinkMoveCommand(game)},
                {new Keys[] {Keys.Z, Keys.N}, new LinkAttackCommand(game)},
                //{new Keys[] {Keys.D1}, new LinkItemCommand(game)},
                //{new Keys[] {Keys.E}, new LinkDamagedCommand(game)}
                {new Keys[] {Keys.Q}, new QuitCommand(game)}

            };
        }

        public Dictionary<Keys[], ICommand> KeyboardCommands
        {
            get => keyboardCommandMap;
        }
    }
}
