using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Util;
using System.Collections.Generic;

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

    // Key: (enemyState, directionX, isMoving)
    // To add a new state, add a new row here. No need to change the whole logic when 
    // adding a new state
    private static readonly Dictionary<(EnemyState, int, bool), SpriteState> StateTable = new()
    {
        { (EnemyState.None,  1,  true),  SpriteState.RightRunning },
        { (EnemyState.None, -1,  true),  SpriteState.LeftRunning  },
        { (EnemyState.None,  1,  false), SpriteState.RightFacing  },
        { (EnemyState.None, -1,  false), SpriteState.LeftFacing   },
        { (EnemyState.Dead,  1,  true),  SpriteState.Dead         },
        { (EnemyState.Dead,  1,  false), SpriteState.Dead         },
        { (EnemyState.Dead, -1,  true),  SpriteState.Dead         },
        { (EnemyState.Dead, -1,  false), SpriteState.Dead         },
    };

    public EnemySprite() => Reset();

    public void Reset()
    {
        currentState = SpriteState.RightFacing;
        ResetFrameState(SourceRects.EnemySourceRects[SpriteState.RightFacing]);
    }

    public void UpdateState(EnemyState enemyState, Vector2 direction, Vector2 velocity)
    {
        int dirX = direction.X >= 0 ? 1 : -1;
        bool isMoving = velocity.X != 0;

        // Fall back to attack states for any unrecognised (attacking) enemy state
        SpriteState newState = StateTable.TryGetValue((enemyState, dirX, isMoving), out SpriteState mapped)
            ? mapped
            : dirX == 1 ? SpriteState.RightAttack : SpriteState.LeftAttack;

        if (currentState != newState)
        {
            currentState = newState;
            ResetFrameState(SourceRects.EnemySourceRects[currentState]);
        }
    }

    public void Update(GameTime gameTime) => UpdateFrame(gameTime);

    public void Draw(SpriteBatch spriteBatch, Vector2 pos)
    {
        spriteBatch.Draw(Assets.PlayerTexture, pos, CurrentSourceRect, Color.Black);
    }
}