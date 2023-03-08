using UnityEngine;

namespace TheIdealShip.Roles;
public class Lover
{
    public static PlayerControl lover1, lover2;
    public static Color Color = new Color32(255, 105, 180, byte.MaxValue);

    public static void clearAndReload()
    {
        lover1 = null;
        lover2 = null;
    }
}