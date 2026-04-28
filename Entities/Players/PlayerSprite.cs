using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint2.Extensions;
using Sprint2.Util;
using System.Collections.Generic;

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

    // Key: (playerState, directionX, isMovingX, isMovingY)
    // To add a new state, add a new row here — no logic changes needed.
    private static readonly Dictionary<(State, int, bool, bool), SpriteState> StateTable = new()
    {
        { (State.None,  1,  false, true),  SpriteState.RightFalling  },
        { (State.None, -1,  false, true),  SpriteState.LeftFalling    },
        { (State.None,  1,  true,  false), SpriteState.RightRunning  },
        { (State.None, -1,  true,  false), SpriteState.LeftRunning   },
        { (State.None,  1,  false, false), SpriteState.RightFacing   },
        { (State.None, -1,  false, false), SpriteState.LeftFacing    },
        { (State.None,  1,  true,  true),  SpriteState.RightFalling  },
        { (State.None, -1,  true,  true),  SpriteState.LeftFalling    },
        { (State.Dead,  1,  false, false), SpriteState.Dead          },
        { (State.Dead,  1,  false, true),  SpriteState.Dead          },
        { (State.Dead,  1,  true,  false), SpriteState.Dead          },
        { (State.Dead,  1,  true,  true),  SpriteState.Dead          },
        { (State.Dead, -1,  false, false), SpriteState.Dead          },
        { (State.Dead, -1,  false, true),  SpriteState.Dead          },
        { (State.Dead, -1,  true,  false), SpriteState.Dead          },
        { (State.Dead, -1,  true,  true),  SpriteState.Dead          }
    };

    public PlayerSprite()
    {
        currentState = SpriteState.RightFacing;
        color = Color.White;
        ResetFrameState(SourceRects.PlayerSourceRects[SpriteState.RightFacing]);
    }

    public void UpdateState(State linkAction, Vector2 direction, Vector2 velocity, bool isDamaged)
    {
        int dirX = direction.X >= 0 ? 1 : -1;
        bool isMovingX = velocity.X != 0;
        bool isMovingY = velocity.Y != 0;

        // Fall back to BlockBreaking for any unrecognised state (e.g. State.BreakBlock)
        SpriteState newState = StateTable.TryGetValue((linkAction, dirX, isMovingX, isMovingY), out SpriteState mapped)
            ? mapped
            : SpriteState.BlockBreaking;

        if (currentState != newState)
        {
            currentState = newState;
            ResetFrameState(SourceRects.PlayerSourceRects[currentState]);
        }

        color = isDamaged ? Color.Red : Color.White;
    }

    public void Update(GameTime gameTime, State linkAction, Vector2 direction, Vector2 velocity, bool isDamaged)
    {
        UpdateState(linkAction, direction, velocity, isDamaged);
        UpdateFrame(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 pos)
    {
        spriteBatch.Draw(Assets.BlockPlayerSpriteSheet, pos, CurrentSourceRect, color);
    }
}