using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.EntityStateMachines;
using Interfaces;
using Sprint2.Sprites;

namespace Sprint2.Player
{
    public class Link : IPlayer
    {
        private Game1 game;

        private LinkSprite linkSprite;
        private LinkStateMachine linkStateMachine;

        public Link(Game1 game)
        {
            Texture2D linkTexture = game.Content.Load<Texture2D>("bin/DesktopGL/sprites/Link");
            linkSprite = new LinkSprite(linkTexture);
            linkStateMachine = new LinkStateMachine(linkSprite);
        }

        public void ChangeDirection(Vector2 direction)
        {
            linkStateMachine.ChangeDirection(direction);
        }

        public void Attack()
        {
            linkStateMachine.Attack();
        }

        public void Update(GameTime gameTime)
        {
            linkStateMachine.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            linkStateMachine.Draw(spriteBatch);
        }
    }
}