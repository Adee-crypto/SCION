using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.EntityStates;
using Sprint2.Interfaces;
using Sprint2.Sprites;

namespace Sprint2.Entities
{
    public class Link : IPlayer
    {
        private Game1 game;

        private LinkSprite linkSprite;
        private LinkStateMachine linkStateMachine;

        public Link(Game1 game)
        {
            Texture2D linkTexture = game.Content.Load<Texture2D>("bin/DesktopGL/sprites/Link");
            linkSprite = new LinkSprite(game.SpriteBatch, linkTexture);
            linkStateMachine = new LinkStateMachine(linkSprite);
        }

        public void ChangeDirection(Vector2 direction)
        {
            linkStateMachine.ChangeDirection(direction);
        }

        public void Update(GameTime gameTime)
        {
            linkStateMachine.Update(gameTime);
        }

        public void Draw()
        {
            linkStateMachine.Draw();
        }
    }
}
