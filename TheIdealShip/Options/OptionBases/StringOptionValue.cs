namespace TheIdealShip.Options.OptionBases;

public class StringOptionBase : OptionBase
{
    public string[] Selection;
    public string StringValue;
    
    public bool EnableTranslation;

    public StringOptionBase(string name, int id, string stringId, optionTab tab) : base(name, id, tab, optionType.String)
    {
        base.StringId = stringId;
    }
    
    public override void GetValue()
    {}

    public override void Increase()
    {
        
    }

    public override void Decrease()
    {
        
    }

    public override void GetValueString()
    {
        
    }
    
}