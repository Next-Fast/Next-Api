
using System.Collections.Generic;

namespace TheIdealShip.Roles.Core;

public static class RoleManager
{
    public static List<RoleBase> RoleBaseS = new List<RoleBase>();
    public static Dictionary<RoleId,SimpleRoleInfo> AllRoleInfo = new(RoleHelpers.AllRoles.Length);
    public static Dictionary<string, RoleBase> RoleBaseDic = new Dictionary<string, RoleBase>();
}