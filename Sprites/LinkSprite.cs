using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Constants;
using Sprint2.Interfaces;


namespace Sprint2.Sprites
{
    public class LinkSprite : ISprite
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
        private SpriteBatch spriteBatch;
        private Texture2D linkTextrue;
        private Rectangle[] frames;
        private int currentFrameIndex;
        private double timeSinceLastFrame;
        private Vector2 position;
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public LinkSprite(SpriteBatch spriteBatch, Texture2D linkTextrue)
        {   
            this.spriteBatch = spriteBatch;
            this.linkTextrue = linkTextrue;
            frames = LinkConstant.GetLinkFrames()[LinkAnimationState.RightFacing];
            currentFrameIndex = 0;
            timeSinceLastFrame = 0;
            position = new Vector2(0, 0);
        }

        public void setFrames(LinkAnimationState linkAnimationState)
        {
            frames = LinkConstant.GetLinkFrames()[linkAnimationState];
            currentFrameIndex = 0;
            timeSinceLastFrame = 0;
        }
        public void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastFrame >= LinkConstant.linkSecondsPerFrame)
            {
                currentFrameIndex = (currentFrameIndex + 1) % frames.Length; // Frames rotate
                timeSinceLastFrame = 0;
            }
        }

        public void Draw()
        {
            spriteBatch.Draw(linkTextrue, position, frames[currentFrameIndex], Color.White);
        }
    }
}
