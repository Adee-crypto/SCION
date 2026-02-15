using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Interfaces;
using Sprint2.Sprites;
using static Sprint2.Sprites.LinkSprite;


namespace Sprint2.EntityStateMachines
{
    public class LinkStateMachine : IEntityStateMachine
    {
        private enum LinkMode
        {
            Still,
            Moving
        };
        private LinkMode linkMode;
        private LinkSprite linkSprite;
        private Vector2 currentDirection;
        private Vector2 currentPosition;
        private float speed;

        public LinkStateMachine(LinkSprite linkSprite)
        {
            linkMode = LinkMode.Still;
            this.linkSprite = linkSprite;
            currentDirection = new Vector2(LinkUtil.linkDefaultXDirection, 0);
            currentPosition = new Vector2(0, 0);
            speed = LinkUtil.linkSpeed;
        }

        public void ChangeDirection(Vector2 newDirection)
        {
            if (newDirection.Equals(currentDirection) && linkMode == LinkMode.Moving) return;

            if (newDirection.X == 0 && newDirection.Y == 0)
            {
                linkMode = LinkMode.Still;
                if (currentDirection.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightFacing);
                if (currentDirection.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftFacing);
                if (currentDirection.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownFacing);
                if (currentDirection.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpFacing);
            }
            else
            {
                currentDirection = newDirection;
                linkMode = LinkMode.Moving;
                if (newDirection.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightRunning);
                if (newDirection.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftRunning);
                if (newDirection.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownRunning);
                if (newDirection.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpRunning);
            }
        }

        public void Attack()
        {
            if (currentDirection.X > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.RightAttack);
            if (currentDirection.X < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.LeftAttack);
            if (currentDirection.Y > 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.DownAttack);
            if (currentDirection.Y < 0) linkSprite.SetFrames(LinkSprite.LinkAnimationState.UpAttack);
        }

        public void Update(GameTime gameTime)
        {
            if (linkMode == LinkMode.Moving)
            {
                currentPosition += currentDirection * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                linkSprite.Position = currentPosition;
            }
            linkSprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            linkSprite.Draw(spriteBatch);
        }
    }
}
