using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Interfaces;


namespace Sprint2.Sprites
{
    public class LinkSprite(Texture2D linkTextrue) : ISprite
    {
        public enum LinkAnimationState
        {
            UpFacing,
            UpRunning,
            DownFacing,
            DownRunning,
            LeftFacing,
            LeftRunning,
            RightFacing,
            RightRunning,
            UpAttack,
            DownAttack,
            LeftAttack,
            RightAttack
        };
        private Texture2D linkTextrue = linkTextrue;
        private Rectangle[] frames = LinkUtil.GetLinkFrames()[LinkAnimationState.RightFacing];
        private int currentFrameIndex = 0;
        private double timeSinceLastFrame = 0;
        private Vector2 position = new Vector2(0, 0);
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public void SetFrames(LinkAnimationState linkAnimationState)
        {
            frames = LinkUtil.GetLinkFrames()[linkAnimationState];
            currentFrameIndex = 0;
            timeSinceLastFrame = 0;
        }
        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastFrame >= LinkUtil.linkSecondsPerFrame)
            {
                currentFrameIndex = (currentFrameIndex + 1) % frames.Length; // Frames rotate
                timeSinceLastFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(linkTextrue, position, frames[currentFrameIndex], Color.White);
        }
    }
}
