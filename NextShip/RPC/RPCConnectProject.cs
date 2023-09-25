using System.Collections.Generic;

namespace NextShip.RPC;

public abstract class RPCConnectProject
{
    public string ID;

    protected RPCConnectProject() =>
        RPCProjectManager.Instance.AllProjects.Add(this);

    public abstract void OnStart();

    public virtual void OnEnd() {}
    
}

public sealed class RPCProjectManager
{
    public List<RPCConnectProject> AllProjects = new ();

    public static RPCProjectManager Instance
    {
        get { return Current ??= new RPCProjectManager(); }
        set => Current = value;
    }

    private static RPCProjectManager Current;

    private readonly RPCProjectManager _instance;

    public RPCProjectManager()
    {
        _instance = this;
        Current = this;
    }
}