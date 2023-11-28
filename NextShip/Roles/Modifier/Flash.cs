using UnityEngine;

namespace NextShip.Roles;

public static class Flash
{
    public static PlayerControl flash;
    public static Color color = new Color32(248, 205, 70, byte.MaxValue);
    public static float speed = 5f;

    public static void clearAndReload()
    {
        flash = null;
    }

    public static void OptionLoad()
    {
    }
}