namespace TheIdealShip.Options.OptionBases;

public class BooleanOptionBase : OptionBase
{
    public bool BooleanValue;
    public string[] BooleanValueSelection = { "false", "true" };

    public BooleanOptionBase(string Title, int id, optionTab tab, optionType type, bool Translation = true) : base(Title, id, tab, type, Translation)
    {
    }

    public BooleanOptionBase(string Title, string stringId, optionTab tab, optionType type, bool Translation = true) : base(Title, stringId, tab, type, Translation)
    {
    }

    public override void Increase()
    {

    }

    public override void Decrease()
    {

    }

    public override void GetValue()
    {

    }

    public override void GetValueString()
    {

    }
}