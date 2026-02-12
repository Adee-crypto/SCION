using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Constants;
using Sprint2.EntityStates;
using Sprint2.Interfaces;
using System;

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
        };
        private LinkAnimationState linkAnimationState;
        private SpriteBatch spriteBatch;
        private Texture2D linkTextrue;
        private Rectangle[] frames;
        private int currentFrameIndex;
        private double timeSinceLastFrame;
        private Vector2 position;

        public LinkSprite(SpriteBatch spriteBatch, Texture2D linkTextrue)
        {   
            linkAnimationState = LinkAnimationState.RightFacing;
            this.spriteBatch = spriteBatch;
            this.linkTextrue = linkTextrue;
            frames = LinkConstant.GetLinkFrames()[LinkAnimationState.RightFacing];
            currentFrameIndex = 0;
            timeSinceLastFrame = 0;
            position = new Vector2(0, 0);
        }

        public void setState(LinkAnimationState linkAnimationState)
        {
            this.linkAnimationState = linkAnimationState;
            currentFrameIndex = 0;
            timeSinceLastFrame = 0;
        }
        public void Update(GameTime gameTime)
        {
            frames = LinkConstant.GetLinkFrames()[linkAnimationState];

            timeSinceLastFrame += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timeSinceLastFrame >= LinkConstant.linkMillisecondsPerFrame)
            {
                currentFrameIndex = (currentFrameIndex + 1) % frames.Length; // Frames rotate
                timeSinceLastFrame = 0;
            }

            position += LinkConstant.GetLinkVelocity()[linkAnimationState];
        }

        public void Draw()
        {
            spriteBatch.Draw(linkTextrue, position, frames[currentFrameIndex], Color.White);
        }
    }
}
