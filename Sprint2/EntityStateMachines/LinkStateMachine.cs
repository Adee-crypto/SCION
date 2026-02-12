using Microsoft.Xna.Framework;
using Sprint2.Constants;
using Sprint2.Interfaces;
using Sprint2.Sprites;

namespace Sprint2.EntityStateMachine
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

        public LinkStateMachine(LinkSprite linkSprite)
        {
            linkMode = LinkMode.Still;
            this.linkSprite = linkSprite;
            currentDirection = new Vector2(LinkConstant.linkDefaultXDirection, 0);
        }

        public void ChangeDirection(Vector2 newDirection)
        {
            if (newDirection.Equals(currentDirection) && linkMode == LinkMode.Moving) return;

            if (newDirection.X == 0 && newDirection.Y == 0)
            {
                linkMode = LinkMode.Still;
                if (currentDirection.X > 0) linkSprite.setState(LinkSprite.LinkAnimationState.RightFacing);
                if (currentDirection.X < 0) linkSprite.setState(LinkSprite.LinkAnimationState.LeftFacing);
                if (currentDirection.Y > 0) linkSprite.setState(LinkSprite.LinkAnimationState.DownFacing);
                if (currentDirection.Y < 0) linkSprite.setState(LinkSprite.LinkAnimationState.UpFacing);
            }
            else
            {
                currentDirection = newDirection;
                linkMode = LinkMode.Moving;
                if (newDirection.X > 0) linkSprite.setState(LinkSprite.LinkAnimationState.RightRunning);
                if (newDirection.X < 0) linkSprite.setState(LinkSprite.LinkAnimationState.LeftRunning);
                if (newDirection.Y > 0) linkSprite.setState(LinkSprite.LinkAnimationState.DownRunning);
                if (newDirection.Y < 0) linkSprite.setState(LinkSprite.LinkAnimationState.UpRunning);
            }
        }

        public void Update(GameTime gameTime)
        {
            linkSprite.Update(gameTime);
        }

        public void Draw()
        {
            linkSprite.Draw();
        }
    }
}
