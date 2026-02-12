using Microsoft.Xna.Framework;
using System.Collections.Generic;
using static Sprint2.Sprites.LinkSprite;


namespace Sprint2.Constants
{
    public class LinkConstant
    {
        public const int linkDefaultXDirection = 1;
        public const int linkDefaultyDirection = 1;
        public const float linkSecondsPerFrame = 0.25f;
        public const float linkSpeed = 100;

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
                { LinkAnimationState.RightRunning, [new(0, 0, 16, 16), new(16, 0, 16, 16)] },
                { LinkAnimationState.UpAttack, [new(32, 32, 16, 16)]},
                { LinkAnimationState.DownAttack, [new(48, 32, 16, 16)]},
                { LinkAnimationState.LeftAttack, [new(16, 32, 16, 16)]},
                { LinkAnimationState.RightAttack, [new(0, 32, 16, 16)]},
            };
            return LinkFramesMap;
        }
    }
}
