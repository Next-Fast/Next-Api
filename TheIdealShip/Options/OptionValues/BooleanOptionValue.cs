using System;
namespace TheIdealShip.Options.OptionValues;

public class BooleanOptionValue : OptionValue<int>
{
    public string[] BooleanValueSelection = {"false", "true"};
    public bool BooleanValue;
    public BooleanOptionValue((int, int, int, int) tuple) : base(tuple)
    {
    }

    public BooleanOptionValue(int defaultValue, int min = 0, int step = 1, int max =1) : base(defaultValue, min, step, max)
    {
        UpdateBoolValue();
    }

    public override void decrease()
    {
        if (Value - Step < Min) return;
        Value -= Step;

        UpdateBoolValue();
    }

    public override int GetValue() => Value;

    public override void increase()
    {
        if (Value + Step > Max) return;
        Value += Step;

        UpdateBoolValue();
    }

    public void UpdateBoolValue() => BooleanValue = Value == 0 ? false : true;

    public string GetBoolStringValue => BooleanValueSelection[Value];
    public bool GetBoolValue => BooleanValue;
}