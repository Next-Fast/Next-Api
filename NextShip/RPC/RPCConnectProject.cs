using System.Collections.Generic;

namespace NextShip.RPC;

public abstract class RPCConnectProject
{
    public string ID;

    protected RPCConnectProject()
    {
        RPCProjectManager.Instance.AllProjects.Add(this);
    }

    public abstract void OnStart();

    public virtual void OnEnd()
    {
    }
}

public sealed class RPCProjectManager
{
    private static RPCProjectManager Current;

    private readonly RPCProjectManager _instance;
    public List<RPCConnectProject> AllProjects = new();

    public RPCProjectManager()
    {
        _instance = this;
        Current = this;
    }

    public static RPCProjectManager Instance
    {
        get { return Current ??= new RPCProjectManager(); }
        set => Current = value;
    }
}