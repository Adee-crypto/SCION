using Interfaces;

namespace Sprint2.Commands
{
    public class PauseCommand(Game1 game) : ICommand
    {
        private Game1 game = game;

        public void Execute(int index)
        {
            game.TogglePause();
        }

        public void Unexecute()
        {
        }
    }
}