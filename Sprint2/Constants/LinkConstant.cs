using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static Sprint2.Sprites.LinkSprite;


namespace Sprint2.Constants
{
    public class LinkConstant
    {
        public const int linkDefaultXDirection = 1;
        public const int linkDefaultyDirection = 1;
        public const double linkMillisecondsPerFrame = 250;
        public static Dictionary<LinkAnimationState, Rectangle[]> GetLinkFrames()
        {
            Dictionary<LinkAnimationState, Rectangle[]> LinkFramesMap = new Dictionary<LinkAnimationState, Rectangle[]>()
            {
                { LinkAnimationState.UpFacing, [new (16, 16, 16, 16)] },
                { LinkAnimationState.UpRunning, [new (0, 16, 16, 16), new(16, 16, 16, 16)] },
                { LinkAnimationState.DownFacing, [new (32, 16, 16, 16)] },
                { LinkAnimationState.DownRunning, [new(32, 16, 16, 16), new(48, 16, 16, 16)] },
                { LinkAnimationState.LeftFacing, [new (32, 0, 16, 16)] },
                { LinkAnimationState.LeftRunning, [new(32, 0, 16, 16), new(48, 0, 16, 16)] },
                { LinkAnimationState.RightFacing, [new (0, 0, 16, 16)] },
                { LinkAnimationState.RightRunning, [new(0, 0, 16, 16), new(16, 0, 16, 16)] }
            };
            return LinkFramesMap;
        }

        public static Dictionary<LinkAnimationState, Vector2> GetLinkVelocity()
        {
            Dictionary<LinkAnimationState, Vector2> LinkVelocity = new Dictionary<LinkAnimationState, Vector2>()
            {
                { LinkAnimationState.UpRunning, new Vector2(0, -1) },
                { LinkAnimationState.UpFacing, new Vector2(0, 0) },
                { LinkAnimationState.DownRunning, new Vector2(0, 1) },
                { LinkAnimationState.DownFacing, new Vector2(0, 0) },
                { LinkAnimationState.LeftRunning, new Vector2(-1, 0) },
                { LinkAnimationState.LeftFacing, new Vector2(0, 0) },
                { LinkAnimationState.RightRunning, new Vector2(1, 0) },
                { LinkAnimationState.RightFacing, new Vector2(0, 0) }
            };
            return LinkVelocity;
        }
    }
}
