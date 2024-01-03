namespace NextShip.Api;

public static class NextPaths
{
    static NextPaths()
    {
        GetPaths();
        CreateDirectory();
    }

    private static void GetPaths()
    {
        RootDirectory = BepInEx.Paths.GameRootPath;
        TIS_DataPath = $"{RootDirectory}/TIS_Data";
        CreativityPath = $"{RootDirectory}/Creativity";
        TIS_TempPath = $"{TIS_DataPath}/TEMP";
        TIS_ConfigPath = $"{TIS_DataPath}/Config";
        TIS_PluginsPath = $"{CreativityPath}/Plugins";
        TIS_TORHats = $"{CreativityPath}/TOR";
    }
    
    
    private static void CreateDirectory()
    {
        var paths = typeof(NextPaths).GetFields().
            Where(n => n.IsStatic && n.IsPublic && n.FieldType == typeof(string)).
            Select(n => n.GetValue(null)).
            Select(n => (string)n);

        foreach (var Path in paths)
        {
            if (Directory.Exists(Path)) continue;
            Directory.CreateDirectory(Path!);
        }
    }

    public static string RootDirectory;
    
    public static string TIS_DataPath { get; private set; }
    
    public static string CreativityPath { get; private set; }
    
    public static string TIS_TempPath { get; private set; }
    
    public static string TIS_ConfigPath { get; private set; }
    
    public static string TIS_PluginsPath { get; private set; }
    
    public static string TIS_TORHats { get; private set; }
}