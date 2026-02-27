using Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Util;
using Sprint2.Lib;

namespace Sprint2.Entities.Enemies;

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

public class EnemySprite : Animated {
    private SpriteState currentState;
    public bool IsDamaged { get; set; }

    public EnemySprite() => Reset();

    public void Reset()
    {
        currentState = SpriteState.RightFacing;
        IsDamaged = false;
        ResetFrameState(SourceRects.EnemySourceRects[SpriteState.RightFacing]);
    }

    public void UpdateState(State enemyState, Vector2 direction, Vector2 velocity, bool isDamaged)
    {
        SpriteState newState;

        if (enemyState == State.None)
        {
            if (velocity.X != 0)
                newState = direction.X == 1 ? SpriteState.RightRunning : SpriteState.LeftRunning;
            else
                newState = direction.X == 1 ? SpriteState.RightFacing : SpriteState.LeftFacing;
        }
        else if (enemyState == State.Dead)
        {
            newState = SpriteState.Dead;
        }
        else if (enemyState == State.Attack)
        {
            newState = direction.X == 1 ? SpriteState.RightAttack : SpriteState.LeftAttack;
        }
        else
        {
            newState = SpriteState.RightFacing;
        }

        if (currentState != newState)
        {
            currentState = newState;
            ResetFrameState(SourceRects.EnemySourceRects[currentState]);
        }
    }

    public void Update(GameTime gameTime) => UpdateFrameState(gameTime);

    public void Draw(SpriteBatch spriteBatch, Vector2 pos)
    {
        spriteBatch.Draw(Assets.PlayerTexture, pos, CurrentSourceRect, IsDamaged ? Color.Red : Color.Black);
    }
}