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
            index %= 4;
            Vector2 direction;
            switch (index)
            {
                case 0:
                    direction = new Vector2(0, -1); // up
                    break;
                case 1:
                    direction = new Vector2(0, 1); // down
                    break;
                case 2:
                    direction = new Vector2(-1, 0); // left
                    break;
                case 3:
                    direction = new Vector2(1, 0); // right
                    break;
                default:
                    direction = new Vector2(0, 0); // never happen
                    break;
            }
            game.player.ChangeDirection(direction);
        }

        public void Unexecute()
        {
            game.player.ChangeDirection(new Vector2(0, 0));
        }
    }
}
