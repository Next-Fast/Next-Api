using System.Collections.Generic;
using System.Linq;
using NextShip.Utilities;

namespace NextShip.Roles.Core;

public static class RoleManager
{
    public static List<RoleBase> AllRoleBase = new();
    public static List<SimpleRoleInfo> AllSimpleRoleInfo = new();
    public static Dictionary<RoleId, SimpleRoleInfo> AllRoleInfo = new(AllRoles.Length);
    public static Dictionary<string, RoleBase> RoleBaseDic = new();
    public static readonly RoleId[] AllRoles = EnumHelper.GetAllValues<RoleId>();

    public static List<SimpleRoleInfo> _AllMainRole = new();
    public static List<SimpleRoleInfo> _AllModifierRole = new();
    public static List<PlayerControl> _AllCrewmate = new();
    public static List<PlayerControl> _AllImpostor = new();
    public static List<PlayerControl> _AllNeutral = new();

    public static void Init()
    {
    }

    public static RoleBase GetRole(this PlayerControl player)
    {
        return AllRoleBase.FirstOrDefault(n => n.Player == player);
    }

    public static void Clear()
    {
    }
}