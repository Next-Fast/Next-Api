using System;
using static TheIdealShip.Languages.LanguageCSV;
using System.IO;
namespace TheIdealShip.Languages
{
    public static class LanguagePack
    {
        // 是否有语言包
        public static bool HPack;
        // 语言文件夹
        public static string LANGUAGEFILE = "Language";
        // 语言文件夹路径
        private static string FPath = $"./{LANGUAGEFILE}";
        // 语言包路径
//        private static string PPath = $"{FPath}/{GetLN()}.dat";
        // 模板文件路径
        private static string LPath = $"{FPath}/lang.dat";
        //初始化
        public static void Init()
        {
            Helpers.CWrite("正在Pack加载中");
            CTT();
        }
        // 获取是否有语言包
  //      public static bool GetHPack()
  //      {
  //          if (Directory.Exists(PPath))
  //          {
  //              File.Delete(LPath);
  //              return true;
  //          }
  //          return false;
  //      }
        // 获取Language名称
        public static string GetLN ()
        {
            var lang = AmongUs.Data.DataManager.settings.language.CurrentLanguage.ToString();
            var name = lang.Replace("SupportedLangs.","");
            return name;
        }
        // 创建文件夹
        private static void CTT()
        {
            if (!Directory.Exists(FPath))
            {
                Helpers.CWrite("不存在Language文件夹,创建文件夹");
                Directory.CreateDirectory(FPath);
            }
            if (!(Directory.GetDirectories(FPath).Length > 0 || Directory.GetFiles(FPath).Length > 0))
            {
                CreateTT();
                Helpers.CWrite("正在创建语言模板");
            }
        }
        // 加载语言包
        public static void LoadPack()
        {
        }
        // 获取语言包文本
//        public static string GetPString(string s)
//        {
//            string r = "";
//            return r;
//        }

        // 创建语言模板
        private static void CreateTT ()
        {
            var text = "";
            foreach (var title in tr)
            {
                text += '"'+$"{title.Key}"+'"' + ":\n";
                File.WriteAllText(LPath,text);
            }
        }
    }
}