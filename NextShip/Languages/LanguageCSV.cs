using System;
using System.Collections.Generic;
using System.Reflection;
using Csv;

namespace NextShip.Languages;

public static class LanguageCSV
{
    public static Dictionary<string, Dictionary<int, string>> translateMaps;

    public static void LoadCSV()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream("NextShip.Resources.Language.string.csv");
        if (stream == null)
        {
            Debug("Csv Not Found");
            return;
        }
        
        translateMaps = new Dictionary<string, Dictionary<int, string>>();

        var options = new CsvOptions
        {
            HeaderMode = HeaderMode.HeaderPresent,
            AllowNewLineInEnclosedFieldValues = false
        };
        foreach (var line in CsvReader.ReadFromStream(stream, options))
        {
            if (line.Values[0][0] is '#' or '#' or '/' or '、') continue;

            try
            {
                Dictionary<int, string> dic = new();

                for (var i = 1; i < line.ColumnCount; i++)
                {
                    var id = int.Parse(line.Headers[i]);
                    dic[id] = line.Values[i].Replace("\\n", "\n").Replace("\\r", "\r");
                }

                if (!translateMaps.TryAdd(line.Values[0], dic))
                    Warn($"LoadCSV:翻译重复在{line.Index}行: \"{line.Values[0]}\"");
            }
            catch (Exception ex)
            {
                Error("加载csv失败\n" + ex, "CSVLoad");
            }
        }
    }

    // 获取CSV文本
    public static string GetCString(string str, SupportedLangs langId)
    {
        var res = $"{str}";

        if (translateMaps.TryGetValue(str, out var dic) &&
            (!dic.TryGetValue((int)langId, out res) || res == "")) //strに該当する&無効なlangIdかresが空
            res = $"{dic[0]}";

        if (string.IsNullOrEmpty(res)) res = $"*{str}";

        return res;
    }
}