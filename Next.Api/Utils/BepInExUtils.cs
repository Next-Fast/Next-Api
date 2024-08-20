using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

namespace Next.Api.Utils;

public static class BepInExUtils
{
    public static PluginInfo GetPlugin(this Assembly assembly)
    {
        return IL2CPPChainloader.Instance.Plugins.FirstOrDefault(n => n.Value.Location == assembly.Location).Value;
    }

    public static ConfigFile GetConfig(this Assembly assembly) => assembly.GetBaseInstance().Config;
    
    public static ManualLogSource GetLogSource(this Assembly assembly) => assembly.GetBaseInstance().Log;

    public static BasePlugin GetBaseInstance(this Assembly assembly) => (BasePlugin)assembly.GetPlugin().Instance;
    
    public static T? GetInstance<T>(this PluginInfo info) where T : BasePlugin => info.Instance as T;

    public static BepInPlugin GetMetadata(this Assembly assembly) => assembly.GetPlugin().Metadata;
}