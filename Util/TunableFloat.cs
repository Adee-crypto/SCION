using System.Threading;
using Sprint2.Extensions;

namespace Sprint2.Util;

public class TunableFloat(float defaultVal) : ITunableFloat
{
    public float DefaultValue { get; } = defaultVal;
    public float Value { get; set; } = defaultVal;

    public void Reset()
    {
        Value = DefaultValue;
    }
}