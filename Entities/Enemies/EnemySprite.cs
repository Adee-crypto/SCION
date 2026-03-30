using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Util;

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

public class EnemySprite : Animated
{
    private SpriteState currentState;
    public EnemySprite() => Reset();

    public void Reset()
    {
        currentState = SpriteState.RightFacing;
        ResetFrameState(SourceRects.EnemySourceRects[SpriteState.RightFacing]);
    }

    public void UpdateState(EnemyState enemyState, Vector2 direction, Vector2 velocity)
    {
        SpriteState newState;

        if (enemyState == EnemyState.None)
        {
            if (velocity.X != 0)
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
        else if (enemyState == EnemyState.Dead)
        {
            newState = SpriteState.Dead;
        }
        else
        {
            if (direction.X == 1) newState = SpriteState.RightAttack;
            else newState = SpriteState.LeftAttack;
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
        spriteBatch.Draw(Assets.PlayerTexture, pos, CurrentSourceRect, Color.Black);
    }
}