global using NextShip.Roles.Core;
using System;
using NextShip.Options;


namespace NextShip.Roles.Core;

public abstract class RoleBase : IDisposable
{
    public RoleBase(PlayerControl player, SimpleRoleInfo simpleRoleInfo)
    {
        SimpleRoleInfo = simpleRoleInfo;
        CanKill = false;
        CanVent = false;
        HasTask = true;

        Player = player;

        var Option = new RoleOptionBase(simpleRoleInfo, -1);

        RoleManager.AllRoleBase.Add(this);
    }

    public PlayerControl Player { get; private set; }
    public SimpleRoleInfo SimpleRoleInfo { get; set; }
    public RoleAction RoleAction { get; set; }
    public RoleEvent RoleEvent { get; set; }
    public bool CanKill { get; }
    public bool CanVent { get; }
    public bool HasTask { get; }

    public void Dispose()
    {
        OnDestroy();
        Player = null;
        RoleManager.AllRoleBase.Remove(this);
    }


    public void setInfo(SimpleRoleInfo info)
    {
        SimpleRoleInfo = info;
    }

    public virtual void OnDestroy()
    {
    }
}