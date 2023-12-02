namespace NextShip.Api.Roles;

public abstract class RoleBase : IDisposable
{
    protected RoleBase(PlayerControl? player)
    {
        CanKill = false;
        CanVent = false;
        HasTask = true;
        WinCheck = () => false;
        Player = player;

        Active = true;
        RoleManager.Get().AllRoleBases.Add(this);
    }

    public PlayerControl? Player { get; private set; }

    public Func<bool> WinCheck { get; }
    public bool CanKill { get; }
    public bool CanVent { get; }
    public bool HasTask { get; }

    public bool Active { get; protected set; }

    public void Dispose()
    {
        OnDestroy();
        Player = null;
        RoleManager.Get().AllRoleBases.Remove(this);
    }

    public virtual void RpcSyncWriter()
    {
    }

    public virtual void RpcSyncReader()
    {
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