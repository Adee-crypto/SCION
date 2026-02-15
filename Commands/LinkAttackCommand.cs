using Microsoft.Xna.Framework;
using Interfaces;


namespace Sprint2.Commands
{
    public class LinkAttackCommand(Game1 game) : ICommand
    {
        private Game1 game = game;

        public void Execute(int index)
        {
            game.player.Attack();
        }

        public void Unexecute()
        {
            game.player.ChangeDirection(-1);
        }
    }
}
