using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;

namespace Sprint2.Entities;

public enum State
{
    LeftFacing,
    LeftRunning,
    RightFacing,
    RightRunning,
    LeftAttack,
    RightAttack,
    LeftFalling,
    RightFalling,
    BlockBreaking,
    Dead
};

public class PlayerSprite : IPlayerSprite {
    private State currentState;
    private Rectangle[] frames;
    private int currentFrameIndex;
    private double timeSinceLastFrame;
    private Color color;
    public Vector2 Position {get;set;}

    public PlayerSprite()
    {
        currentState = State.RightFacing;
        frames = SourceRects.PlayerSourceRects[State.RightFacing];
        currentFrameIndex = 0;
        timeSinceLastFrame = 0;
        color = Color.White;
    }

    public void SetFrames(PlayerState playerState, Vector2 direction, Vector2 velocity, bool isDamaged)
    {
        color = isDamaged ? Color.Red : Color.White;
        State newState;

        if (playerState == PlayerState.None)
        {
            if (velocity.Y != 0)
                newState = direction.X == 1 ? State.RightFalling : State.LeftFalling;
            else if (velocity.X != 0)
                newState = direction.X == 1 ? State.RightRunning : State.LeftRunning;
            else
                newState = direction.X == 1 ? State.RightFacing : State.LeftFacing;
        }
        else if (playerState == PlayerState.Dead)
        {
            newState = State.Dead;
        }
        else if (playerState == PlayerState.Attack)
        {
            newState = direction.X == 1 ? State.RightAttack : State.LeftAttack;
        }
        else
        {
            newState = State.BlockBreaking;
        }

        if (currentState != newState)
        {
            currentState = newState;
            frames = SourceRects.PlayerSourceRects[newState];
            currentFrameIndex = 0;
            timeSinceLastFrame = 0;
        }
    }

    public void Update(GameTime gameTime)
    {
        timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;
        while (timeSinceLastFrame >= Consts.playerFrameTime) //ADD FUNC TO FUNCS TO AUTOMATE ALL LOOPS LIKE THIS
        {
            currentFrameIndex = (currentFrameIndex + 1) % frames.Length; // Frames rotate
            timeSinceLastFrame -= Consts.playerFrameTime;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D texture) {
        spriteBatch.Draw(texture, Position, frames[currentFrameIndex], color);
    }
}