using System;
using System.Collections.Generic;

namespace NextShip.Plugins;

public abstract class ShipPlugin
{
    public ShipPluginInfo ShipPluginInfo;
    public List<PluginCompatibility> PluginCompatibilities = new ();
    
    public ShipPlugin(){}
    public ShipPlugin(ShipPluginInfo pluginInfo)
    {
        ShipPluginInfo = pluginInfo;
    }

    public abstract void Load();
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class PluginCompatibility : Attribute
{
    public string PluginName;
    public CompatibilityLevel Compatibility;

    public PluginCompatibility(string pluginName, CompatibilityLevel compatibility = CompatibilityLevel.Compatible)
    {
        PluginName = pluginName;
        Compatibility = compatibility;        
    }
}

public enum CompatibilityLevel
{
    Unknown = 0,
    Compatible = 1,
    Incompatible = 2,
    Dependency = 3,
    Need = 4
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class ShipPluginInfo : Attribute
{
    public ShipPluginInfo(string Id, Version Version, string Name)
    {
        this.Id = Id;
        this.Version = Version;
        this.Name = Name;
    }

    public string Id { get; }
    public Version Version { get; }
    public string Name { get; }
}