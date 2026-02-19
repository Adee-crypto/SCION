using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Interfaces;
using System.Collections.Generic;


namespace Sprint2.Sprites;

public class LinkSprite : ISprite
{
    public enum LinkAnimationState
    {
        LeftFacing,
        LeftRunning,
        RightFacing,
        RightRunning,
        LeftAttack,
        RightAttack,
        LeftFalling,
        RightFalling
    };
    private LinkAnimationState currentState;
    private Rectangle[] frames;
    private int currentFrameIndex;
    private double timeSinceLastFrame;
    public Vector2 Position {get;set;}

    public LinkSprite()
    {
        currentState = LinkAnimationState.RightFacing;
        frames = LinkUtil.GetFrames()[LinkAnimationState.RightFacing];
        currentFrameIndex = 0;
        timeSinceLastFrame = 0;
    }

    public void SetFrames(LinkAnimationState linkAnimationState)
    {
        if (linkAnimationState != currentState) {
            currentState = linkAnimationState;
            frames = LinkUtil.GetFrames()[linkAnimationState];
            currentFrameIndex = 0;
            timeSinceLastFrame = 0;
        }
    }

    public void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;
        if (timeSinceLastFrame >= LinkUtil.secondsPerFrame){
            currentFrameIndex = (currentFrameIndex + 1) % frames.Length; // Frames rotate
            timeSinceLastFrame = 0;
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(LinkUtil.linkTexture, Position, frames[currentFrameIndex], Color.White);
    }
}