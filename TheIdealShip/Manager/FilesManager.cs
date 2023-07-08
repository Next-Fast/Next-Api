using System.IO;

namespace TheIdealShip.Manager;

public class FilesManager
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

    public static bool CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Msg("创建文件夹:" + path.TextRemove("./") + "成功", filename: "FilesManager");
        }
        else
        {
            Msg("文件夹已存在", filename: "FilesManager");
        }


        return !Directory.Exists(path);
    }

    public static bool GetDataDirectory(string name) => CreateDirectory(Path.Combine(TIS_DataPath, name));
    public static bool GetRootDirectory(string name) => CreateDirectory($"./{name}");
    public static bool GetCreativityDirectory(string name) => CreateDirectory(Path.Combine(CreativityPath, name));
}