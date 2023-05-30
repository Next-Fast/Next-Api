global using TheIdealShip.Roles.Core;

namespace TheIdealShip.Roles.Core;

public class RoleBase
{
    public string RoleName { get; }
    public RoleId roleid { get; }
    public RoleInfo info { get; }
    public bool CanKill { get; }
    public bool CanVent { get; }
    public bool HasTask { get; }

    public RoleBase(string role, RoleId roleId)
    {
        RoleName = role;
        roleid = roleId;
        info = null;
        CanKill = false;
        CanVent = false;
        HasTask = true;

        RoleManager.RoleBaseS.Add(this);

    }

/*     public virtual void OptionLoad(){} */
}