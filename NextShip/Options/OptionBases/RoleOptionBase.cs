namespace NextShip.Options;

public class RoleOptionBase : StringOptionBase
{
    public static string[] defaultChances =
        { "0% ", "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%" };

    public RoleOptionBase(SimpleRoleInfo simpleRoleInfo, int id, optionTab RoleTab = default, string[] chances = null) :
        base(simpleRoleInfo.Name, id, defaultChances,
            optionTab.other)
    {
        type = optionType.Role;
        if (chances != null) Selection = chances;
        if (RoleTab != default)
            tab = RoleTab;
        else
            tab = simpleRoleInfo.roleTeam switch
            {
                RoleTeam.Crewmate => optionTab.Crewmate,
                RoleTeam.Impostor => optionTab.Impostor,
                RoleTeam.Neutral => optionTab.Neutral,
                _ => optionTab.other
            };
    }

    public override RoleOptionBase GetBase()
    {
        return this;
    }
}