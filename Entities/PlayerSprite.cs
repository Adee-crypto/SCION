using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using Sprint2.Lib;

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

public class PlayerSprite : Animated, IEntitySprite {
    private State currentState;
    private Color color;

    public PlayerSprite()
    {
        currentState = State.RightFacing;
        color = Color.White;
        ResetFrameState(SourceRects.PlayerSourceRects[State.RightFacing]);
    }

    public void SetFrames(PlayerState linkAction, Vector2 direction, Vector2 velocity, bool isDamaged)
    {
        color = isDamaged ? Color.Red : Color.White;
        State newState;

        if (linkAction == PlayerState.None)
        {
            if (velocity.Y != 0)
                newState = direction.X == 1 ? State.RightFalling : State.LeftFalling;
            else if (velocity.X != 0)
                newState = direction.X == 1 ? State.RightRunning : State.LeftRunning;
            else
                newState = direction.X == 1 ? State.RightFacing : State.LeftFacing;
        }
        else if (linkAction == PlayerState.Dead)
        {
            newState = State.Dead;
        }
        else if (linkAction == PlayerState.Attack)
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
            ResetFrameState(SourceRects.PlayerSourceRects[currentState]);
        }
    }

    public void Update(GameTime gameTime) => UpdateFrameState(gameTime);

    public void Draw(SpriteBatch spriteBatch, Vector2 pos)
    {
        spriteBatch.Draw(Assets.PlayerTexture, pos, CurrentSourceRect, color);
    }
}