using Microsoft.Xna.Framework;

namespace Sprint2.Extensions; //This should be changed

public class Ticker(double freq)
{
    public double Remainder { get; set; }
    public int TickAge { get; set; }
    public double Freq { get; set; } = freq;

    public void Reset()
    {
        TickAge = 0;
        Remainder = 0;
    }

    /// <summary>effictively an update method, must be called for fields to update</summary>
    public int TicksPassed(GameTime gameTime)
    {
        Remainder += gameTime.ElapsedGameTime.TotalSeconds;
        int NewTicks = (int)(Remainder / Freq);
        Remainder %= Freq;
        TickAge += NewTicks;
        return NewTicks;
    }
}