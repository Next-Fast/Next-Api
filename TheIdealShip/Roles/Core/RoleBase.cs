global using TheIdealShip.Roles.Core;
using System;


namespace TheIdealShip.Roles.Core;

public abstract class RoleBase : IDisposable
{
    public RoleBase(PlayerControl player)
    {
        CanKill = false;
        CanVent = false;
        HasTask = true;

        Player = player;
        SimpleRoleInfo.setBase(this);

        RoleManager.AllRoleBase.Add(this);
    }

    public PlayerControl Player { get; private set; }
    public SimpleRoleInfo SimpleRoleInfo { get; set; }
    public bool CanKill { get; }
    public bool CanVent { get; }
    public bool HasTask { get; }

    public void Dispose()
    {
        OnDestroy();
        Player = null;
    }

    public void setInfo(SimpleRoleInfo info)
    {
        SimpleRoleInfo = info;
    }

    public virtual void OnDestroy()
    {
    }
}