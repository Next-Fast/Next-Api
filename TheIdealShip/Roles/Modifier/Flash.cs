using UnityEngine;

namespace TheIdealShip.Roles;

public static class Flash
{
    public static PlayerControl flash;
    public static Color color = new Color32(248, 205, 70, byte.MaxValue);
    public static float speed = 5f;

    public static void clearAndReload()
    {
        flash = null;
        speed = flashSpeed.getFloat();
    }

    public static void OptionLoad()
    {
        flashSpawnRate = CustomOption.Create(100, Types.Modifier, cs(color, "Flash"), rates, null, true);
        flashSpeed = CustomOption.Create(101, Types.Modifier, "Speed", 5f, 1f, 10f, 0.5f, flashSpawnRate);
    }
}