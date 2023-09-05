global using NextShip.Roles.Core;
using System;
using NextShip.Options;
using NextShip.Utilities.Attributes;


namespace NextShip.Roles.Core;

public abstract class RoleBase : IDisposable
{
    protected RoleBase(PlayerControl player)
    {
        CanKill = false;
        CanVent = false;
        HasTask = true;
        WinCheck = () => false;
        Player = player;
        
        Active = true;
        RoleManager.Get().AllRoleBases.Add(this);
    }

    public PlayerControl Player { get; private set; }
    
    public Func<bool> WinCheck { get; }
    public bool CanKill { get; }
    public bool CanVent { get; }
    public bool HasTask { get; }
    
    public bool Active { get; }

    public void Dispose()
    {
        OnDestroy();
        Player = null;
        RoleManager.Get().AllRoleBases.Remove(this);
    }

    protected virtual void OnDestroy()
    {
    }
}