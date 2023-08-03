namespace NextShip.Options;

public class IntOptionBase : OptionBase
{
    private IntOptionValue _intOptionValue;
    public IntOptionBase(string Title, int id, optionTab tab, IntOptionValue intOptionValue, bool Translation = true) : base(Title, id, tab, optionType.Int, Translation)
    {
        _intOptionValue = intOptionValue;
        OptionManager.AllIntOption.Add(this);
    }

    public override void Increase() => _intOptionValue.GetValue();

    public override void Decrease() => _intOptionValue.GetValue();

    public override int GetInt()
    {
        return _intOptionValue.GetValue();
    }

    public override float GetFloat()
    {
        return _intOptionValue.GetValue();
    }

    public override string GetValueString()
    {
        return _intOptionValue.GetValue().ToString();
    }

    public override OptionBase GetBase() => this;
}