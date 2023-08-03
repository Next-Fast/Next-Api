using System;

namespace NextShip.Plugins;

[AttributeUsage(AttributeTargets.Class)]
public abstract class ShipPlugin : Attribute
{
    public ShipPluginInfo ShipPluginInfo;
    public ShipPlugin(ShipPluginInfo pluginInfo)
    {
        ShipPluginInfo = pluginInfo;
    }
    
    public ShipPlugin(string Id, Version Version, string Name)
    {
        ShipPluginInfo = new ShipPluginInfo(Id, Version, Name);
    }
    
    public abstract void Load();
}

public sealed class ShipPluginInfo : Attribute
{
    public string Id { get;  }
    public Version Version { get; }
    public string Name { get; }
    
    public ShipPluginInfo(string Id, Version Version, string Name)
    {
        this.Id = Id;
        this.Version = Version;
        this.Name = Name;
    }
}