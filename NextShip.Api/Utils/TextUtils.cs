using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

namespace NextShip.Api.Utils;

public static class TextUtils
{
    public static string TextRemove(this string Otext, string targetText)
    {
        return Otext.Replace(targetText, "");
    }

    public static string TextRemove(this string Otext, string[] targetText)
    {
        foreach (var tt in targetText) Otext = Otext.Replace(tt, "");
        return Otext;
    }

    public static string cs(Color c, string s)
    {
        return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b),
            ToByte(c.a), s);
    }

    public static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte)(f * 255);
    }

    public static string RemoveBlank(this string s)
    {
        return s.Replace(" ", "");
    }

    public static string clearColor(this string str)
    {
        var s = str.Replace("</color>", "");
        var found = s.IndexOf(">", StringComparison.Ordinal);
        s = s.Substring(found + 1);
        return s;
    }

    public static string ToColorString(this string text, Color color)
    {
        string colorString;
        colorString = "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + text + "</color>";
        return colorString;
    }

    public static string ToColorString(this string text, System.Drawing.Color color)
    {
        string colorString;
        colorString = "<color=#" + ColorTranslator.ToHtml(color) + ">" + text + "</color>";
        return colorString;
    }


    public static string Get_Key(this string text)
    {
        var text2 = string.Empty;
        var add = false;
        foreach (var @char in text)
        {
            if (text == "%")
            {
                add = !add;
                continue;
            }

            text2 += @char;
        }

        return text2;
    }

    public static string CombinePath(this string path, params string[] Paths)
    {
        var paths = new List<string> { path };
        paths.AddRange(Paths);
        return Path.Combine(paths.ToArray());
    }

    public static string ToStringText(this char c)
    {
        return $"{c}";
    }

    public static string ToText(this IEnumerable<string> strings)
    {
        return strings.Aggregate("", (current, s) => current + s);
    }

    public static string ToText(this IEnumerable<char> chars)
    {
        return chars.Aggregate("", (current, c) => current + c);
    }
}