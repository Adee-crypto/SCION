using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel.Design;

namespace Sprint2.Sprites;

public class PlayerSprite : IPlayerSprite
{
    private PlayerUtil.PlayerAnimation currentState;
    private Rectangle[] frames;
    private int currentFrameIndex;
    private double timeSinceLastFrame;
    private Color color;
    public Vector2 Position {get;set;}

    public PlayerSprite()
    {
        currentState = PlayerUtil.PlayerAnimation.RightFacing;
        frames = PlayerUtil.framesMap[PlayerUtil.PlayerAnimation.RightFacing];
        currentFrameIndex = 0;
        timeSinceLastFrame = 0;
        color = Color.White;
    }

    public void SetFrames(PlayerUtil.PlayerState state, Vector2 direction, Vector2 velocity, bool isDamaged)
    {
        color = isDamaged ? Color.Red : Color.White;
        PlayerUtil.PlayerAnimation newState;

        if (state == PlayerUtil.PlayerState.None)
        {
            if (velocity.Y != 0)
                newState = direction.X == 1 ? PlayerUtil.PlayerAnimation.RightFalling : PlayerUtil.PlayerAnimation.LeftFalling;
            else if (velocity.X != 0)
                newState = direction.X == 1 ? PlayerUtil.PlayerAnimation.RightRunning : PlayerUtil.PlayerAnimation.LeftRunning;
            else
                newState = direction.X == 1 ? PlayerUtil.PlayerAnimation.RightFacing : PlayerUtil.PlayerAnimation.LeftFacing;
        }
        else if (state == PlayerUtil.PlayerState.Dead)
        {
            newState = PlayerUtil.PlayerAnimation.Dead;
        }
        else if (state == PlayerUtil.PlayerState.Attack)
        {
            newState = direction.X == 1 ? PlayerUtil.PlayerAnimation.RightAttack : PlayerUtil.PlayerAnimation.LeftAttack;
        }
        else
        {
            newState = PlayerUtil.PlayerAnimation.BlockBreaking;
        }

        if (currentState != newState)
        {
            currentState = newState;
            frames = PlayerUtil.framesMap[newState];
            currentFrameIndex = 0;
            timeSinceLastFrame = 0;
        }
    }

    public void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;
        if (timeSinceLastFrame >= PlayerUtil.secondsPerFrame)
        {
            currentFrameIndex = (currentFrameIndex + 1) % frames.Length; // Frames rotate
            timeSinceLastFrame = 0;
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(PlayerUtil.playerTexture, Position, frames[currentFrameIndex], color);
    }
}