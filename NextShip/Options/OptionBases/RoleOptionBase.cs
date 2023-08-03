namespace  NextShip.Options;

public class RoleOptionBase : StringOptionBase
{
    public static string[] defaultChances = new []{ "0% ", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%"};
    public RoleOptionBase(string name, int id, optionTab tab, string[] chances = null) : base(name, id, defaultChances, tab)
    {
        type = optionType.Role;
        if (chances != null)
        {
            Selection = chances;
        }
        OptionManager.AllRoleOption.Add(this);    
    }
    public override RoleOptionBase GetBase() => this;
}

