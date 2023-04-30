namespace TheIdealShip.Roles;

public class RoleBase
{
    public string RoleName { get; }
    public RoleId roleid { get; }
    public bool CanKill { get; }
    public bool CanVent { get; }
    public bool HasTask { get; }

    public RoleBase(string role, RoleId roleId)
    {
        RoleName = role;
        roleid = roleId;
        CanKill = false;
        CanVent = false;
        HasTask = true;
    }
}