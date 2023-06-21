using System.ComponentModel;
using System.Collections.Generic;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using csv = TheIdealShip.Languages.LanguageCSV;
using System.IO;
using TheIdealShip.Manager;

namespace TheIdealShip.Languages
{
    public class LanguagePack
    {

        // 语言文件夹
        public static string LANGUAGEFILE = "Language";
        public static string languageName = "SChinese";
        // 语言文件夹路径
        private static string FPath = $"./{LANGUAGEFILE}";
        // 语言包路径
        public static string PPath = $"{FPath}/{languageName}.dat";
        // 模板文件路径
        private static string LPath = $"{FPath}/lang.dat";
        //初始化
        public static void Init()
        {
            Msg("正在Pack加载中", "Language Pack");
            GetLN();
            CTT();
        }
        // 获取Language名称
        public static void GetLN ()
        {
            var lang = AmongUs.Data.DataManager.Settings.language.CurrentLanguage.ToString();
            var name = lang.Replace("SupportedLangs.","");
            languageName = name;
            Info("Language:" + name, "Language Pack");
        }
        // 语言转序号
        public static int GetLangInt()
        {
            var lang = AmongUs.Data.DataManager.Settings.language.CurrentLanguage;
            return lang switch
            {
                SupportedLangs.English => 0,
                SupportedLangs.Latam => 1,
                SupportedLangs.Brazilian => 2,
                SupportedLangs.Portuguese => 3,
                SupportedLangs.Korean => 4,
                SupportedLangs.Russian => 5,
                SupportedLangs.Dutch => 6,
                SupportedLangs.Filipino => 7,
                SupportedLangs.French => 8,
                SupportedLangs.German => 9,
                SupportedLangs.Italian => 10,
                SupportedLangs.Japanese => 11,
                SupportedLangs.Spanish => 12,
                SupportedLangs.SChinese => 13,
                SupportedLangs.TChinese => 14,
                SupportedLangs.Irish => 15,
                _ => 0,
            };
        }
        // 序号转语言英文名
        public static string GetLname(int id) => id switch
        {
            0 => "English",
            1 => "Latam",
            2 => "Brazilian",
            3 => "Portuguese",
            4 => "Korean",
            5 => "Russian",
            6 => "Dutch",
            7 => "Filipino",
            8 => "French",
            9 => "German",
            10 => "Italian",
            11 => "Japanese",
            12 => "Spanish",
            13 => "SChinese",
            14 => "TChinese",
            15 => "Irish",
            _ => "",
        };

        // 创建文件夹
        private static void CTT()
        {
            FilesManager.CreateDirectory(FPath);
            if (!(Directory.GetDirectories(FPath).Length > 0 || Directory.GetFiles(FPath).Length > 0) || !File.Exists(LPath) || !File.Exists(PPath))
            {
                CreateTT();
                Msg("正在创建语言模板", "Language Pack");
            }
        }

        // 创建语言模板
        private static void CreateTT ()
        {
            var text = "";
            foreach (var title in csv.tr)
            {
                text += '"'+$"{title.Key}"+'"' + " : "+'"'+LanguageCSV.GetCString(title.Key,0)+'"'+"\n";
                File.WriteAllText(LPath,text);
            }
        }

        static private LanguagePack language = null;
        static private Dictionary<string, string> defaultLanguageSet = new Dictionary<string, string>();

        public Dictionary<string, string> languageSet;


        public static string GetPString(string key)
        {
            if (language?.languageSet.ContainsKey(key) ?? false)
            {
                return language.languageSet[key];
            }
            return "*" + key;
        }

        public static bool CheckValidKey(string key)
        {
            return language?.languageSet.ContainsKey(key) ?? true;
        }

        public static void AddDefaultKey(string key, string format)
        {
            defaultLanguageSet.Add(key, format);
            if (!CheckValidKey(key)) language.languageSet.Add(key, format);
        }


        public LanguagePack()
        {
            languageSet = new Dictionary<string, string>(defaultLanguageSet);
        }

        public static void LoadDefaultKey()
        {
            AddDefaultKey("option.empty", "");
        }
        public static void Load()
        {
            string lang = languageName;


            language = new LanguagePack();
            language.deserialize(@"Language\" + lang + ".dat");

        }

        public bool deserialize(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path, Encoding.GetEncoding("utf-8")))
                {
                    return deserialize(sr);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool deserialize(StreamReader reader)
        {
            bool result = true;
            try
            {
                string data = "", line;


                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length < 3)
                    {
                        continue;
                    }
                    if (data.Equals(""))
                    {
                        data = line;
                    }
                    else
                    {
                        data += "," + line;
                    }
                }


                if (!data.Equals(""))
                {


                    JsonSerializerOptions option = new JsonSerializerOptions
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                        WriteIndented = true
                    };

                    var deserialized = JsonSerializer.Deserialize<Dictionary<string, string>>("{ " + data + " }", option);
                    foreach (var entry in deserialized)
                    {
                        languageSet[entry.Key] = entry.Value;
                    }

                    result = true;
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}
