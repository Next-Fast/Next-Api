using UnityEngine;

namespace NextShip.Roles;

public class Lover
{
    public static PlayerControl lover1, lover2;
    public static Color Color = new Color32(255, 105, 180, byte.MaxValue);
    public static bool suicide;

    public static void clearAndReload()
    {
        lover1 = null;
        lover2 = null;
        suicide = false;
    }

    public static void OptionLoad()
    {

    }
}