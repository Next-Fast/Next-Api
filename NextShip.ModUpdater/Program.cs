namespace NextShip.ModUpdater;

public static class Program
{
    public  static List<Updater> AllUpdater = new();
    
    public static void Main(string[] args)
    {
        var Option = args[0].Replace("-", "") switch
        {
            "1" => UpdateOption.Mod,
            "2" => UpdateOption.BepInEx,
            "3" => UpdateOption.All,
            _ => UpdateOption.None
        };
        
        if (Option is UpdateOption.Mod or UpdateOption.All)
            AllUpdater.Add(new ModUpdater());

        if (Option is UpdateOption.BepInEx or UpdateOption.All)
            AllUpdater.Add(new BepInExUpdater());
        
        Update(args);
    }

    private static async void Update(string[] args)
    {
        GetVersion(args, out var version);
        if (version == null) return;
        
        foreach (var Updater in AllUpdater)
        {
           await Updater.Update(version);
        }
    }

    public static bool GetVersion(string[] args, out List<(string, UpdateOption)>? Version)
    {
        Version = null;
        var Versions = args.Where(n => n.Contains("Version")).ToList();
        if (Versions.Count == 0) return false;
        Version = new List<(string, UpdateOption)>();
        foreach (var ver in Versions)
        {
            var index = ver.IndexOf(":", StringComparison.Ordinal);
            var subString = ver.Substring(index + 1);
            var option = UpdateOption.None;
            if (ver.Contains("Mod")) option = UpdateOption.Mod;
            if (ver.Contains("BepInEx")) option = UpdateOption.BepInEx;
            Version.Add((subString, option));
        }

        return true;
    }
    
    public enum UpdateOption
    {
        None,
        Mod,
        BepInEx,
        All
    }
}

public abstract class Updater
{
    public abstract Task Update(List<(string, Program.UpdateOption)> Version);
}

