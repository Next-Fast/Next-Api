using HarmonyLib;

namespace NextShip.Api.Roles;

public partial class RoleManager
{
    public static RoleManager Instance;
    public readonly List<Role> Roles = new();

    public List<RoleBase> AllRoleBases = new();
    public List<SimpleRoleInfo> AllSimpleRoleInfos = new();


    public static RoleManager Get()
    {
        return Instance ??= new RoleManager();
    }

    public List<RoleBase> GetActiveRole()
    {
        return AllRoleBases.Where(n => n.Active).ToList();
    }

    public void RegisterRole(Role role)
    {
        Roles.Add(role);
    }

    public void RegisterRole(IEnumerable<Role> roles)
    {
        roles.Do(RegisterRole);
    }


    public T GetRole<T>(PlayerControl playerControl) where T : RoleBase
    {
        return (T)AllRoleBases.FirstOrDefault(n => n.Player == playerControl)!;
    }
}