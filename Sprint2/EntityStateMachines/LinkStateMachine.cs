using Microsoft.Xna.Framework;
using Sprint2.Constants;
using Sprint2.Interfaces;
using Sprint2.Sprites;


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
            currentDirection = new Vector2(LinkConstant.linkDefaultXDirection, 0);
            currentPosition = new Vector2(0, 0);
            speed = LinkConstant.linkSpeed;
        }

        public void ChangeDirection(Vector2 newDirection)
        {
            if (newDirection.Equals(currentDirection) && linkMode == LinkMode.Moving) return;

            if (newDirection.X == 0 && newDirection.Y == 0)
            {
                linkMode = LinkMode.Still;
                if (currentDirection.X > 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.RightFacing);
                if (currentDirection.X < 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.LeftFacing);
                if (currentDirection.Y > 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.DownFacing);
                if (currentDirection.Y < 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.UpFacing);
            }
            else
            {
                currentDirection = newDirection;
                linkMode = LinkMode.Moving;
                if (newDirection.X > 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.RightRunning);
                if (newDirection.X < 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.LeftRunning);
                if (newDirection.Y > 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.DownRunning);
                if (newDirection.Y < 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.UpRunning);
            }
        }

        public void attack()
        {
            if (currentDirection.X > 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.RightAttack);
            if (currentDirection.X < 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.LeftAttack);
            if (currentDirection.Y > 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.DownAttack);
            if (currentDirection.Y < 0) linkSprite.setFrames(LinkSprite.LinkAnimationState.UpAttack);
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

        public void Draw()
        {
            linkSprite.Draw();
        }
    }
}
