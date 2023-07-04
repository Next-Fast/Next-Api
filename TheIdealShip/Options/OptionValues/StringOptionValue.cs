namespace TheIdealShip.Options.OptionValues;

public class StringOptionValue : OptionValue<int>
{
    public string[] Selection;
    public string StringValue;
    public StringOptionValue((int, int, int, int) tuple) : base(tuple)
    {
    }

    public StringOptionValue(string[] selection, int defaultValue, int min = 0, int step = 1 , int max = 0) : base(defaultValue, min, step, max)
    {
        Selection = selection;
        max = selection.Length;
        StringValue = selection[defaultValue];
    }

    public override void decrease()
    {
        if (Value - Step < Min) return;
        Value -= Step;
        UpdateStringValue();
    }

    public override int GetValue() => Value;

    public override void increase()
    {
        if (Value + Step > Max) return;
        Value += Step;
        UpdateStringValue();
    }

    public string GetStringValue() => Selection[Value];
    public void UpdateStringValue() => StringValue = Selection[Value];
}