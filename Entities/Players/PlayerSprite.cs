using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using Sprint2.Lib;

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

public class PlayerSprite : Animated {
    private SpriteState currentState;
    public bool IsDamaged { get; set; }

    public PlayerSprite()
    {
        currentState = SpriteState.RightFacing;
        IsDamaged = false;
        ResetFrameState(SourceRects.PlayerSourceRects[SpriteState.RightFacing]);
    }

    public void UpdateState(State linkAction, Vector2 direction, Vector2 velocity, bool isDamaged)
    {
        SpriteState newState;

        if (linkAction == State.None)
        {
            if (velocity.Y != 0)
                newState = direction.X == 1 ? SpriteState.RightFalling : SpriteState.LeftFalling;
            else if (velocity.X != 0)
                newState = direction.X == 1 ? SpriteState.RightRunning : SpriteState.LeftRunning;
            else
                newState = direction.X == 1 ? SpriteState.RightFacing : SpriteState.LeftFacing;
        }
        else if (linkAction == State.Dead)
        {
            newState = SpriteState.Dead;
        }
        else if (linkAction == State.Attack)
        {
            newState = direction.X == 1 ? SpriteState.RightAttack : SpriteState.LeftAttack;
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
    }

    public void Update(GameTime gameTime) => UpdateFrameState(gameTime);

    public void Draw(SpriteBatch spriteBatch, Vector2 pos)
    {
        spriteBatch.Draw(Assets.PlayerTexture, pos, CurrentSourceRect, IsDamaged ? Color.Red : Color.White);
    }
}