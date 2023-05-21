using System.Collections.Generic;
namespace TheIdealShip.Roles;

public class RoleBase
{
    public static List<RoleBase> RoleBaseS = new List<RoleBase>();

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

        RoleBaseS.Add(this);
    }

/*     public virtual void OptionLoad(){} */
}

public static class RoleBaseVoid
{

    public static RoleBase GetRoleBase(this RoleId id)
    {
        RoleBase roleBase = RoleBase.RoleBaseS.Find(x => x.roleid == id);
        return roleBase;
    }
}