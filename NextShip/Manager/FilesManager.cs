using System.IO;

namespace NextShip.Manager;

public static class FilesManager
{
    public enum FileType
    {
        Csv,
        Json,
        Yaml,
        Txt
    }

    public const string TIS_DataPath = "./TIS_Data";
    public const string CreativityPath = "./Creativity";
    public const string TIS_TempPath = "./TIS_Data/TEMP";
    public const string TIS_ConfigPath = TIS_DataPath + "/Config";


    public static void Init()
    {
        CreateDirectory(TIS_DataPath);
        CreateDirectory(CreativityPath);
        CreateDirectory(TIS_TempPath);
        CreateDirectory(TIS_ConfigPath);
    }

    public static DirectoryInfo CreateDirectory(string path)
    {
        DirectoryInfo directoryInfo;
        if (!Directory.Exists(path))
        {
            directoryInfo = Directory.CreateDirectory(path!);
            Msg("创建文件夹:" + path.TextRemove("./") + "成功", filename: "FilesManager");
        }
        else
        {
            directoryInfo = new DirectoryInfo(path);
            Msg("文件夹已存在", filename: "FilesManager");
        }


        return directoryInfo;
    }

    public static DirectoryInfo GetDataDirectory(string name)
    {
        return CreateDirectory(Path.Combine(TIS_DataPath, name));
    }

    public static DirectoryInfo GetRootDirectory(string name)
    {
        return CreateDirectory($"./{name}");
    }

    public static DirectoryInfo GetCreativityDirectory(string name)
    {
        return CreateDirectory(Path.Combine(CreativityPath, name));
    }

    public static DirectoryInfo GetConfigDirectory(string name)
    {
        return CreateDirectory(Path.Combine(CreativityPath, "Config", name));
    }

    public static DirectoryInfo GetCosmeticsCacheDirectory(Cosmetics.CosmeticType type)
    {
        var directoryName = type switch
        {
            Cosmetics.CosmeticType.NamePlate => "CacheNamePlate",
            Cosmetics.CosmeticType.Visor => "CacheVisor",
            Cosmetics.CosmeticType.Skin => "CacheSkin",
            Cosmetics.CosmeticType.Hat => "CacheHat",
            _ => string.Empty
        };
        return CreateDirectory(Path.Combine(CreativityPath, directoryName));
    }

    public static string Is(this string text, FileType type)
    {
        return text += type switch
        {
            FileType.Csv => ".csv",
            FileType.Json => ".json",
            FileType.Yaml => ".yaml",
            FileType.Txt => ".txt",
            _ => ""
        };
    }
}