using Next.Api.Attributes;

namespace Next.Api.Bases;

public abstract class ShipPlugin
{
    public List<PluginCompatibility> PluginCompatibilities = [];
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