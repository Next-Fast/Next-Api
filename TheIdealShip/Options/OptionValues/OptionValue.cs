namespace TheIdealShip.Options.OptionValues;

public abstract class OptionValue<T>
{
    public T DefaultValue;
    public T Value;
    public T Min;
    public T Step;
    public T Max;
    public OptionValue
    (
        T defaultValue,
        T min,
        T step,
        T max
    )
    {
        this.DefaultValue = defaultValue;
        this.Min = min;
        this.Step = step;
        this.Max = max;

        Value = defaultValue;
    }

    public OptionValue( (T, T, T, T) tuple) : this(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4) {}

    public abstract T GetValue();
    public abstract void increase();
    public abstract void decrease();
}