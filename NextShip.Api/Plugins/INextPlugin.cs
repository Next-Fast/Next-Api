namespace NextShip.Api.Plugins;

public interface INextPlugin
{
    public void OnLoad() {}

    public virtual bool OnEnable() => true;
    
    public virtual bool OnDisable() => true;

    public void OnUnload() {}
}