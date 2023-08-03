namespace NextShip.Options;

public class BooleanOptionBase : StringOptionBase
{
    public bool BooleanValue;
    public static string[] BooleanValueSelection = { "false", "true" };

    public BooleanOptionBase(string Title, int id, optionTab tab, bool Translation = true) : base(Title, id, BooleanValueSelection, tab)
    {
        type = optionType.Boolean;
        IntOptionValue = new IntOptionValue(0, 0, 1,1);
        OptionManager.AllBooleanOption.Add(this);
    }

    public override BooleanOptionBase GetBase() => this;
}