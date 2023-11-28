namespace NextShip.Api.Utils;

public static class MathFUtils
{
    public static bool Range(this int num, int min, int max)
    {
        return num >= min && num <= max;
    }
}