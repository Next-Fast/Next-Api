namespace NextShip.Api.Roles;

public partial class RoleManager
{
    private RoleAssigner Assigner => RoleAssigner.Get();

    public IEnumerator<RoleBase?> GetAssignRole(PlayerControl player)
    {
        Assign:
        Assigner.AssignRole();

        if (!Assigner.Get(player, out var role))
            goto Assign;

        yield return role;
    }
}

public class RoleAssigner
{
    private static RoleAssigner Instance;

    private Random Random = new();

    public List<RoleBase?> AllAssigns { private set; get; } = new();

    public static RoleAssigner Get()
    {
        return Instance ??= new RoleAssigner();
    }

    public bool Get(PlayerControl player, out RoleBase? role)
    {
        role = AllAssigns.FirstOrDefault(n => n?.Player == player);

        return role != null;
    }

    public void AssignRole()
    {
        var Roles = RoleManager.Get().Roles.Where(n => n.EnableAssign).ToList();

        foreach (var @base in from player in CachedPlayer.AllPlayers
                 where
                     player.CanAssaign()
                     &&
                     Get(player, out _)
                 let role = Roles[GetValue()]
                 select
                     role.CreateRoleBase(player))
            AllAssigns.Add(@base);

        return;

        int GetValue()
        {
            return Random.Next(0, Roles.Count - 1);
        }
    }

    public void Restore()
    {
        AllAssigns = new List<RoleBase?>();
        Random = new Random();
    }
}