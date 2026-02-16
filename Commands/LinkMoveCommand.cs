using Microsoft.Xna.Framework;
using Interfaces;


namespace Sprint2.Commands
{
    public class LinkMoveCommand : ICommand
    {
        private Game1 game;

        public LinkMoveCommand(Game1 game)
        {
            this.game = game;
        }

        public void Execute(int index)
        {
            game.Player.ChangeDirection(index);
        }

        public void Unexecute()
        {
            game.Player.ChangeDirection(-1);
        }
    }
}
