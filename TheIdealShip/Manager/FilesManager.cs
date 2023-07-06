using System.IO;

namespace TheIdealShip.Manager;

public class FilesManager
{
    public const string TIS_DataPath = "./TIS_Data";
    public const string CreativityPath = "./Creativity";


    public static void Init()
    {
        CreateDirectory(TIS_DataPath);
        CreateDirectory(CreativityPath);
    }

    public static void CreateDirectory(string path)
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
    }
}