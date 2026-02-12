using Microsoft.Xna.Framework;
using Sprint2.Interfaces;


namespace Sprint2.Commands
{
    public class LinkAttackCommand : ICommand
    {
        private Game1 game;

        public LinkAttackCommand(Game1 game)
        {
            this.game = game;
        }

        public void Execute(int index)
        {
            game.Player.attack();
        }

        public void Unexecute()
        {
            game.Player.ChangeDirection(new Vector2(0, 0));
        }
    }
}
