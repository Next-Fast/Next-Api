using System;

namespace NextShip.Plugins;

public abstract class ShipPlugin
{
    public string Id { get;  }
    public Version Version { get; }
    public string Name { get; }
    
    public ShipPlugin(ShipPluginInfo pluginInfo)
    {
        Id = pluginInfo.Id;
        Version = pluginInfo.Version;
        Name = pluginInfo.Name;
    }
    
    public ShipPlugin(string Id, Version Version, string Name)
    {
        this.Id = Id;
        this.Version = Version;
        this.Name = Name;
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