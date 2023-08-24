using System.Reflection;

namespace NextShip.Utilities;

public class AssetLoader
{
    private Assembly __Assembly;
    
    public AssetLoader(Assembly assembly)
    {
        __Assembly = assembly;
    }
}