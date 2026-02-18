using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Interfaces;
using System.Collections.Generic;


namespace Sprint2.Sprites
{
    public class LinkSprite : ISprite
    {
        public enum LinkAnimationState
        {
            LeftFacing,
            LeftRunning,
            RightFacing,
            RightRunning,
            LeftAttack,
            RightAttack
        };

        private Rectangle[] frames = LinkUtil.GetFrames()[LinkAnimationState.RightFacing];
        private int currentFrameIndex = 0;
        private double timeSinceLastFrame = 0;
        private Vector2 position;

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public void SetFrames(LinkAnimationState linkAnimationState)
        {
            frames = LinkUtil.GetFrames()[linkAnimationState];
            currentFrameIndex = 0;
            timeSinceLastFrame = 0;
        }
        public void Update(GameTime gameTime, IEnumerable<Rectangle> objects)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastFrame >= LinkUtil.secondsPerFrame)
            {
                currentFrameIndex = (currentFrameIndex + 1) % frames.Length; // Frames rotate
                timeSinceLastFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(LinkUtil.texture, position, frames[currentFrameIndex], Color.White);
        }
    }
}
