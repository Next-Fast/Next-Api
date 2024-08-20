using System.Text.RegularExpressions;
using UnityEngine;

namespace Next.Api.Utils;

public static class ColorUtils
{
    public static Color32 HTMLColorTo32(this string HTML_Color)
    {
        var regex = new Regex("^#?([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$");
        if (ColorUtility.TryParseHtmlString(HTML_Color, out var color) && regex.IsMatch(HTML_Color)) return color;

        return new Color32(255, 255, 255, byte.MinValue);
    }
}