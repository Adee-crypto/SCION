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
            Vector2 direction = index switch
            {
                0 => new Vector2(0, -1),// up
                1 => new Vector2(0, 1),// down
                2 => new Vector2(-1, 0),// left
                3 => new Vector2(1, 0),// right
                _ => new Vector2(0, 0),// never happen
            };

            game.player.ChangeDirection(direction);
        }

        public void Unexecute()
        {
            game.player.ChangeDirection(new Vector2(0, 0));
        }
    }
}
