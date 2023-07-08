using UnityEngine;

namespace TheIdealShip.Roles;

public class Jester
{
    public static PlayerControl jester;
    public static Color color = new Color32(255, 105, 180, byte.MaxValue);
    public static bool triggerJesterWin = false;
    public static bool CanCallEmergency = true;

    public static void clearAndReload()
    {
        jester = null;
        CanCallEmergency = jesterCanCallEmergency.getBool();
    }

    public static void OptionLoad()
    {
        jesterSpawnRate = CustomOption.Create(150, Types.Neutral, cs(color, "Jester"), rates, null, true);
        jesterCanCallEmergency = CustomOption.Create(151, Types.Neutral, "CanCallEmergency", true, jesterSpawnRate);
    }
}