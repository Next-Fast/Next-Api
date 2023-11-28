namespace NextShip.Api.Roles;

public abstract class Role
{
    protected Type RoleBaseType = null!;
    protected Type RoleType = null!;

    public SimpleRoleInfo SimpleRoleInfo { get; protected set; } = null!;

    public List<RoleBase> RoleBase { get; set; } = null!;

    public List<PlayerControl> PlayerControls { get; set; } = null!;


    public bool EnableAssign { get; set; }

    public Func<PlayerControl, RoleBase> CreateRoleBase { get; set; } = null!;

    public virtual void OptionLoad()
    {
    }

    public virtual bool CanCreate()
    {
        return true;
    }
}