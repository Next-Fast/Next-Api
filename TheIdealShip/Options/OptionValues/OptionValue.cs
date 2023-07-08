namespace TheIdealShip.Options.OptionValues;

public abstract class OptionValue<T>
{
    public T DefaultValue;
    public T Max;
    public T Min;
    public T Step;
    public T Value;

    public OptionValue
    (
        T defaultValue,
        T min,
        T step,
        T max
    )
    {
        DefaultValue = defaultValue;
        Min = min;
        Step = step;
        Max = max;

        Value = defaultValue;
    }

    public OptionValue((T, T, T, T) tuple) : this(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4)
    {
    }

    public abstract T GetValue();
    public abstract void increase();
    public abstract void decrease();
}