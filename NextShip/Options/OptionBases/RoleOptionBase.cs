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
        throw new System.NotImplementedException();
    }

    public override void Decrease()
    {
        throw new System.NotImplementedException();
    }

    public override void GetValue()
    {
        throw new System.NotImplementedException();
    }

    public override void GetValueString()
    {
        throw new System.NotImplementedException();
    }

    public override RoleOptionBase GetBase() => this;
}

