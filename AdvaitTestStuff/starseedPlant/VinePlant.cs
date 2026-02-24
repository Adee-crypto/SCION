using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Advait;

public class VinePlant : Plant
{
    public VinePlant(Texture2D texture, Point startPosition)
        : base(texture, startPosition)
    {
    }

    protected override Point[] GetPatternOffsets()
    {
        // Pattern:
        //  1
        //  1
        // 111
        //  1

        return
        [
            new Point(0,0),
            new Point(0,1),
            new Point(-1,2),
            new Point(0,2),
            new Point(1,2),
            new Point(0,3)
        ];
    }
}
