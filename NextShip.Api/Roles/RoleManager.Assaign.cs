namespace NextShip.Api.Roles;

public partial class RoleManager
{
    private RoleAssigner Assigner => RoleAssigner.Get();

    public IEnumerator<RoleBase?> GetAssignRole(PlayerControl player)
    {
        Assign:

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


    public void Restore()
    {
        AllAssigns = new List<RoleBase?>();
        Random = new Random();
    }
}