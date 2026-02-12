using Microsoft.Xna.Framework;
using Sprint2.Interfaces;
using System;

namespace Sprint2.Command
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
            index = index % 4;
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
            game.Player.ChangeDirection(direction);
        }

        public void StopExecute()
        {
            game.Player.ChangeDirection(new Vector2(0, 0));
        }
    }
}
