using HarmonyLib;

namespace NextShip.Api.Roles;

public partial class RoleManager
{
    public static RoleManager Instance;

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


    public T GetRole<T>(PlayerControl playerControl) where T : RoleBase
    {
        return (T)AllRoleBases.FirstOrDefault(n => n.Player == playerControl)!;
    }
}