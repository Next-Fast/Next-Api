namespace NextShip.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class PluginCompatibility : Attribute
{
    public CompatibilityLevel Compatibility;
    public string PluginName;

    public PluginCompatibility(string pluginName, CompatibilityLevel compatibility = CompatibilityLevel.Compatible)
    {
        PluginName = pluginName;
        Compatibility = compatibility;
    }
}