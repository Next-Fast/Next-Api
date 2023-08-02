namespace NextShip.Options;

public class BooleanOptionBase : OptionBase
{
    public bool BooleanValue;
    public string[] BooleanValueSelection = { "false", "true" };
    public IntOptionValue IntOptionValue;

    public BooleanOptionBase(string Title, int id, optionTab tab, optionType type, bool Translation = true) : base(Title, id, tab, type, Translation)
    {
        IntOptionValue = new IntOptionValue(0, 0, 1,1);
    }

    public BooleanOptionBase(string Title, string stringId, optionTab tab, optionType type, bool Translation = true) : base(Title, stringId, tab, type, Translation)
    {
        IntOptionValue = new IntOptionValue(0, 0, 1,1);
    }

    public override void Increase()
    {

    }

    public override void Decrease()
    {

    }

    public override int GetInt()
    {
        return IntOptionValue.GetValue();
    }

    public override float GetFloat()
    {
        return 0;
    }

    public override string GetValueString()
    {
        return BooleanValueSelection[IntOptionValue.GetValue()];
    }

    public override BooleanOptionBase GetBase() => this;
}