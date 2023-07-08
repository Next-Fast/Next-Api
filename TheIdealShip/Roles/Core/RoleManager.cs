using System.Collections.Generic;
using System.Linq;
using TheIdealShip.Utilities;

namespace TheIdealShip.Roles.Core;

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

    public static RoleBase GetRoleBase(this PlayerControl player)
    {
        return AllRoleBase.Where(n => n.Player == player).FirstOrDefault();
    }
}