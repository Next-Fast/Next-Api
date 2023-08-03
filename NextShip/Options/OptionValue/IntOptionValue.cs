using System;

namespace NextShip.Options;

public class IntOptionValue : OptionValue<int>
{


    public IntOptionValue(int defaultValue, int min, int step, int max) : base(defaultValue, min, step, max)
    {
    }

    public override void decrease()
    {
        if (Value - Step < Min) return;

        Value -= Step;
    }

    public override int GetValue() => Value;

    public override void increase()
    {
        if (Value + Step > Min) return;

        Value += Step;
    }
}