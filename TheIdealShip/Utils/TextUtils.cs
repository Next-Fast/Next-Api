using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

namespace TheIdealShip.Utils;

public static class TextUtils
{
    public static string TextRemove(this string Otext, string targetText)
    {
        return Otext.Replace(targetText, "");
    }

    public static string TextRemove(this string Otext, string[] targetText)
    {
        foreach (var tt in targetText) Otext.Replace(tt, "");
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
        var found = s.IndexOf(">");
        s = s.Substring(found + 1);
        return s;
    }

    public static string ToColorString(this string text, Color color)
    {
        string colorString;
        colorString = "<color=" + ColorUtility.ToHtmlStringRGB(color) + ">" + text + "<color/>";
        return colorString;
    }

    public static string ToColorString(this string text, System.Drawing.Color color)
    {
        string colorString;
        colorString = "<color=" + ColorTranslator.ToHtml(color) + ">" + text + "<color/>";
        return colorString;
    }
}