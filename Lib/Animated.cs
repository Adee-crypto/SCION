using Microsoft.Xna.Framework;

namespace Sprint2.Lib; //This should be changed

public abstract class Animated
{
    public int FrameIndex {get; private set;}
    public Ticker Ticker { get; } = new(0.2);
    public Rectangle[] FrameSourceRects { get; private set; }
    public Rectangle CurrentSourceRect => FrameSourceRects[FrameIndex];
    
    public void ResetFrameState(Rectangle[] newFrames) {
        Ticker.Reset();
        FrameIndex = 0;
        FrameSourceRects = newFrames;
    }

    public void UpdateFrameState(GameTime gameTime) {
        FrameIndex = (FrameIndex + Ticker.TicksPassed(gameTime)) % FrameSourceRects.Length;
    }
}