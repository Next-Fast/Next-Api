using UnityEngine;

namespace TheIdealShip.Roles;

public static class Sheriff
{
    public static PlayerControl sheriff;
    public static Color color = new Color32(248, 205, 70, byte.MaxValue);
    public static float shootNumber = 5f;
    public static float cooldown = 30f;
    public static PlayerControl currentTarget;

    public static void clearAndReload()
    {
        sheriff = null;
        currentTarget = null;
        cooldown = sheriffCooldown.getFloat();
        shootNumber = sheriffshootNumber.getFloat();
    }

    public static void OptionLoad()
    {
        sheriffSpawnRate = CustomOption.Create(20, Types.Crewmate, cs(color, "Sheriff"), rates, null, true);
        sheriffCooldown =
            CustomOption.Create(21, Types.Crewmate, "SheriffCooldown", 30f, 10f, 60f, 2.5f, sheriffSpawnRate);
        sheriffshootNumber = CustomOption.Create(22, Types.Crewmate, "ShootNumber", 5f, 1f, 15f, 1f, sheriffSpawnRate);
    }
}