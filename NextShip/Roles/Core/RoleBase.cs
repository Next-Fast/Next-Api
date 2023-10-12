global using NextShip.Roles.Core;
using System;
using NextShip.Game.GameEvents;
using NextShip.RPC;


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

    public Role ParentRole { get; protected set; }
    public PlayerControl Player { get; private set; }

    public Func<bool> WinCheck { get; }
    public bool CanKill { get; }
    public bool CanVent { get; }
    public bool HasTask { get; }

    public bool Active { get; protected set; }
    protected FastRPC FastRPC { get; set; }

    public void Dispose()
    {
        OnDestroy();
        Player = null;
        RoleManager.Get().AllRoleBases.Remove(this);
    }

    public virtual void RpcSyncWriter()
    { 
        var fast = FastRPC ??= new FastRPC();
        
        fast.Writer.Write((byte)CustomRPC.SetRole);
        fast.Writer.Write(Player.PlayerId);
        fast.Writer.Write(ParentRole.SimpleRoleInfo.RoleStringId);
    }

    public virtual void RpcSyncReader()
    {
        var fast = FastRPC ??= new FastRPC();
    }

    public virtual IGameEvent GetGameEvent()
    {
        return null;
    }
    
    public virtual void OnStart()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnTeamShow()
    {
    }

    public virtual void OnRoleShow()
    {
    }

    public virtual void OnMeetingStart()
    {
    }

    public virtual void OnExile()
    {
    }

    public virtual void OnExileEnd()
    {
    }

    protected virtual void OnDestroy()
    {
    }
}