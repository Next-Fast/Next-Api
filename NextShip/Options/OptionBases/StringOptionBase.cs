namespace NextShip.Options;

public class StringOptionBase : OptionBase
{
    public string[] Selection;
    public string StringValue;
    public IntOptionValue IntOptionValue;

    public bool StringTranslation;

    public StringOptionBase(string name, int id, string[] selection, optionTab tab) : base(name, id, tab, optionType.String)
    {
        IntOptionValue = new IntOptionValue(0, 0, 1, selection.Length);
        Selection = selection;
        OptionManager.AllStringOption.Add(this);    
    }

    public override int GetInt()
    {
        return IntOptionValue.GetValue();
    }

    public override float GetFloat()
    {
        return IntOptionValue.GetValue();
    }
    

    public override string GetValueString()
    {
        return Selection[IntOptionValue.GetValue()];
    }
    
    public override void Increase() => IntOptionValue.increase();

    public override void Decrease() => IntOptionValue.decrease();

    public override StringOptionBase GetBase() => this;
}