global using TheIdealShip.Roles.Core;

using System;


namespace TheIdealShip.Roles.Core;

public class RoleBase : IDisposable
{
    public PlayerControl Player { get; private set; }
    public SimpleRoleInfo SimpleRoleInfo { get; }
    public bool CanKill { get; }
    public bool CanVent { get; }
    public bool HasTask { get; }

    public RoleBase(string role, RoleId roleId)
    {
        CanKill = false;
        CanVent = false;
        HasTask = true;

        RoleManager.RoleBaseS.Add(this);

    }

    public void Dispose()
    {
        OnDestroy();
        Player = null;
    }

    public virtual void OnDestroy()
    { }
}