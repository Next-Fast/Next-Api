namespace NextShip.Api.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ShipPluginInfo : Attribute
{
    public ShipPluginInfo(string Id, ShipVersion Version, string Name)
    {
        this.Id = Id;
        this.Version = Version;
        this.Name = Name;
    }

    public string Id { get; }
    public ShipVersion Version { get; }
    public string Name { get; }
}