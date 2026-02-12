using Sprint2.Interfaces;

namespace Sprint2.Commands
{
    public class QuitCommand : ICommand
    {
        private Game1 game;

        public QuitCommand(Game1 game)
        {
            this.game = game;
        }

        public void Execute(int index)
        {
            game.Exit();
        }

        public void Unexecute()
        {
        }
    }
}
