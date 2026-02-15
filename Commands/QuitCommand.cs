using Interfaces;

namespace Sprint2.Commands
{
    public class QuitCommand(Game1 game) : ICommand
    {
        private Game1 game = game;

        public void Execute(int index)
        {
            game.Exit();
        }

        public void Unexecute()
        {
        }
    }
}
