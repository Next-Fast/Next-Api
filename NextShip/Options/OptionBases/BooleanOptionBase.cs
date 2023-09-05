namespace NextShip.Options;

public class BooleanOptionBase : StringOptionBase
{
    private static readonly string[] BooleanValueSelection = { "false", "true" };
    public bool BooleanValue;

    public BooleanOptionBase(string Title, int id, optionTab tab, bool Translation = true) : base(Title, id,
        BooleanValueSelection, tab)
    {
        type = optionType.Boolean;
        IntOptionValue = new IntOptionValue(0, 0, 1, 1);
    }

    public override BooleanOptionBase GetBase()
    {
        return this;
    }
}