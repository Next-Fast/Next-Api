namespace  NextShip.Options;

public class RoleOptionBase : OptionBase
{
    public RoleOptionBase(string Title, int id, optionTab tab, optionType type, bool Translation = true) : base(Title, id, tab, type, Translation)
    {
    }

    public RoleOptionBase(string Title, string stringId, optionTab tab, optionType type, bool Translation = true) : base(Title, stringId, tab, type, Translation)
    {
    }

    public override void Increase()
    {
    }

    public override void Decrease()
    {
    }

    public override int GetInt()
    {
        return 1;
    }

    public override float GetFloat()
    {
        return 1f;
    }

    public override string GetValueString()
    {
        return "";
    }

    public override RoleOptionBase GetBase() => this;
}

