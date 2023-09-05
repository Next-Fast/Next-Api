using System;
using System.IO;

namespace NextShip.Manager;

public static class FilesManager
{
    public const string TIS_DataPath = "./TIS_Data";
    public const string CreativityPath = "./Creativity";
    public const string TIS_TempPath = "./TIS_Data/TEMP";


    public static void Init()
    {
        CreateDirectory(TIS_DataPath);
        CreateDirectory(CreativityPath);
        CreateDirectory(TIS_TempPath);
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
    
    public enum FileType
    {
        Csv,
        Json,
        Yaml,
        Txt
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