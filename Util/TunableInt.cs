using Sprint2.Extensions;

namespace Sprint2.Util;

public class TunableInt(int defaultVal) : ITunableInt
{
    public int DefaultValue { get; } = defaultVal;
    public int Value { get; set; } = defaultVal;

    public void Reset()
    {
        Value = DefaultValue;
    }
}