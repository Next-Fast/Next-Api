namespace TheIdealShip.Utils;

public static class TextUtils
{
    public static string TextRemove(this string Otext, string targetText) => Otext.Replace(targetText, "");
    public static string TextRemove(this string Otext, string[] targetText)
    {
        foreach (var tt in targetText)
        {
            Otext.Replace(tt, "");
        }
        return Otext;
    }
}