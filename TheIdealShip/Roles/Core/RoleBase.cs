global using TheIdealShip.Roles.Core;

using System;


namespace TheIdealShip.Roles.Core;

public abstract class RoleBase : IDisposable
{
    public PlayerControl Player { get; private set; }
    public SimpleRoleInfo SimpleRoleInfo { get; set;}
    public bool CanKill { get; }
    public bool CanVent { get; }
    public bool HasTask { get; }

    public RoleBase(PlayerControl player)
    {
        CanKill = false;
        CanVent = false;
        HasTask = true;

        this.Player = player;
        SimpleRoleInfo.setBase(this);

        RoleManager.AllRoleBase.Add(this);
    }

    public void setInfo(SimpleRoleInfo info) => SimpleRoleInfo = info;

    public void Dispose()
    {
        OnDestroy();
        Player = null;
    }

    public virtual void OnDestroy()
    { }
}