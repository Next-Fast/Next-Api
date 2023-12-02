namespace NextShip.Api.Bases;


public abstract class ShipPlugin
{
    public List<PluginCompatibility> PluginCompatibilities = new();
    public ShipPluginInfo ShipPluginInfo = null!;

    public ShipPlugin()
    {
    }

    public ShipPlugin(ShipPluginInfo pluginInfo)
    {
        ShipPluginInfo = pluginInfo;
    }

    public abstract void Load();
}