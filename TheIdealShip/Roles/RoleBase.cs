global using RoleBase = TheIdealShip.Roles.RoleBase;

using System.Collections.Generic;
namespace TheIdealShip.Roles;

public class RoleBase
{
    public static List<RoleBase> RoleBaseS = new List<RoleBase>();

    public string RoleName { get; }
    public RoleId roleid { get; }
    public bool CanKill { get; }
    public bool CanVent { get; }
    public bool HasTask { get; }

    public List<CustomButton> Buttons = new List<CustomButton>();
    public List<CustomOption> Options = new List<CustomOption>();

    public RoleBase(string role, RoleId roleId)
    {
        RoleName = role;
        roleid = roleId;
        CanKill = false;
        CanVent = false;
        HasTask = true;

        Buttons = null;
        Options = null;

        RoleBaseS.Add(this);
    }
}

public static class RoleBaseVoid
{

    public static RoleBase GetRoleBase(this RoleId id)
    {
        RoleBase roleBase = RoleBase.RoleBaseS.Find(x => x.roleid == id);
        return roleBase;
    }
}