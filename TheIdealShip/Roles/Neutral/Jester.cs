using UnityEngine;

namespace TheIdealShip.Roles;
public class Jester
{
    public static PlayerControl jester;
    public static Color color = new Color32(255,105,180,byte.MaxValue);
    public static bool triggerJesterWin = false;
    public static bool CanCallEmergency = true;
    public static void clearAndReload()
    {
        jester = null;
        CanCallEmergency = CustomOptionHolder.jesterCanCallEmergency.getBool();
    }
}