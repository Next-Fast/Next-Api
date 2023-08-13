using System.Globalization;

namespace NextShip.Options;

public class FloatOptionBase : OptionBase
{
    private readonly FloatOptionValue _floatOptionValue;

    public FloatOptionBase(string Title, int id, optionTab tab, FloatOptionValue floatOptionValue,
        bool Translation = true) : base(Title, id, tab, optionType.Float, Translation)
    {
        _floatOptionValue = floatOptionValue;
        OptionManager.AllFloatOption.Add(this);
    }


    public override void Increase()
    {
        _floatOptionValue.increase();
    }

    public override void Decrease()
    {
        _floatOptionValue.decrease();
    }

    public override int GetInt()
    {
        return (int)_floatOptionValue.GetValue();
    }

    public override float GetFloat()
    {
        return _floatOptionValue.GetValue();
    }

    public override string GetValueString()
    {
        return _floatOptionValue.GetValue().ToString(CultureInfo.CurrentCulture);
    }

    public override OptionBase GetBase()
    {
        return this;
    }
}