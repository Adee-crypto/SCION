using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Util;

namespace Sprint2.Entities.Players;

public enum SpriteState
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

public class PlayerSprite : Animated
{
    private SpriteState currentState;
    private Color color;

    public PlayerSprite()
    {
        currentState = SpriteState.RightFacing;
        color = Color.White;
        ResetFrameState(SourceRects.PlayerSourceRects[SpriteState.RightFacing]);
    }

    public void UpdateState(State linkAction, Vector2 direction, Vector2 velocity, bool isDamaged)
    {
        SpriteState newState;
        if (linkAction == State.None)
        {
            if (velocity.Y != 0)
            {
                if (direction.X == 1) newState = SpriteState.RightFalling;
                else newState = SpriteState.LeftFacing;

            }
            else if (velocity.X != 0)
            {
                if (direction.X == 1) newState = SpriteState.RightRunning;
                else newState = SpriteState.LeftRunning;

            }
            else
            {
                if (direction.X == 1) newState = SpriteState.RightFacing;
                else newState = SpriteState.LeftFacing;
            }
        }
        else if (linkAction == State.Dead)
        {
            newState = SpriteState.Dead;
        }
        else if (linkAction == State.Attack)
        {
            if (direction.X == 1) newState = SpriteState.RightAttack;
            else newState = SpriteState.LeftAttack;

        }
        else
        {
            newState = SpriteState.BlockBreaking;
        }

        if (currentState != newState)
        {
            currentState = newState;
            ResetFrameState(SourceRects.PlayerSourceRects[currentState]);
        }

        if (isDamaged) color = Color.Red;
        else color = Color.White;
    }

    public void Update(GameTime gameTime) => UpdateFrameState(gameTime);

    public void Draw(SpriteBatch spriteBatch, Vector2 pos)
    {
        spriteBatch.Draw(Assets.PlayerTexture, pos, CurrentSourceRect, color);
    }
}