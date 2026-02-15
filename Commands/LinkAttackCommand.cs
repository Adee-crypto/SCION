using Microsoft.Xna.Framework;
using Interfaces;


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
            game.player.Attack();
        }

        public void Unexecute()
        {
            game.player.ChangeDirection(new Vector2(0, 0));
        }
    }
}
