using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace NextShip.Roles;

public class RoleManager
{
    public static RoleManager Instance;

    public static RoleManager Get() => Instance ??= new RoleManager();

    public List<RoleBase> AllRoleBases = new ();
    public List<SimpleRoleInfo> AllSimpleRoleInfos = new();
    public readonly List<Role> Roles = new();

    public List<RoleBase> GetActiveRole() => AllRoleBases.Where(n => n.Active).ToList();

    public void RegisterRole(Role role) => Roles.Add(role);
    public void RegisterRole(IEnumerable<Role> roles) => roles.Do(RegisterRole);

}