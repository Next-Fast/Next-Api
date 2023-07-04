namespace TheIdealShip.Options.OptionValues;
public class IntOptionValue : OptionValue<int>
{
    public IntOptionValue((int, int, int, int) tuple) : base(tuple)
    {
    }

    public IntOptionValue(int defaultValue, int min, int step, int max) : base(defaultValue, min, step, max)
    {
    }

    public override void decrease()
    {
        throw new System.NotImplementedException();
    }

    public override int GetValue()
    {
        throw new System.NotImplementedException();
    }

    public override void increase()
    {
        throw new System.NotImplementedException();
    }
}