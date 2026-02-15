using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.EntityStateMachines;
using Interfaces;
using Sprint2.Sprites;

namespace Sprint2.Player
{
    public class Link : IPlayer
    {
        private LinkSprite linkSprite;
        private LinkStateMachine linkStateMachine;

        public Link()
        {
            linkSprite = new LinkSprite();
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