using System.Threading;
using Sprint2.Extensions;

namespace Sprint2.Util;

public class TunableFloat : ITunable
{
    public float DefaultValue { get; }
    public float Value { get; set; }

    public TunableFloat(float defaultVal)
    {
        DefaultValue = defaultVal;
        Value = defaultVal;
    }

    public void Reset()
    {
        Value = DefaultValue;
    }
}