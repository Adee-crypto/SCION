using Microsoft.Xna.Framework;

namespace Sprint2.Util; //This should be changed

public abstract class Animated
{
    public int FrameIndex {get; private set;}
    public float Age { get; private set; }
    public float Time {get; private set;}
    public Rectangle[] FrameSourceRects { get; private set; }
    public Rectangle CurrentSourceRect => FrameSourceRects[FrameIndex];
    
    public void ResetFrameState(Rectangle[] newFrames) {
        FrameIndex = 0;
        Age = 0;
        Time = 0;
        FrameSourceRects = newFrames;
    }

    public void UpdateFrameState(GameTime gameTime)
    {
        Time = (float) gameTime.ElapsedGameTime.TotalSeconds;
        int indexIncrease = (int) ((Age % 0.2 + Time) / 0.2); //make this customizeable somehow
        FrameIndex = (FrameIndex + indexIncrease) % FrameSourceRects.Length;
        Age += Time;
    }
}