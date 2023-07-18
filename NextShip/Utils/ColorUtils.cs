using System.Text.RegularExpressions;
using UnityEngine;

namespace NextShip.Utils;

public static class ColorUtils
{
    public static Color32 HTMLColorTo32(this string HTMLcolor)
    {
        Regex regex = new("^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$");
        if (ColorUtility.TryParseHtmlString(HTMLcolor, out var color) && regex.IsMatch(HTMLcolor)) return color;

        return new Color32(255, 255, 255, byte.MinValue);
    }
}