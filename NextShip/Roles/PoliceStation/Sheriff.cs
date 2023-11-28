using UnityEngine;

namespace NextShip.Roles;

public class Sheriff : RoleBase
{
    public static PlayerControl sheriff;
    public static Color color = new Color32(248, 205, 70, byte.MaxValue);
    public static float shootNumber = 5f;
    public static float cooldown = 30f;
    public static PlayerControl currentTarget;


    public Sheriff(PlayerControl player) : base(player)
    {
    }


    public static void clearAndReload()
    {
        sheriff = null;
        currentTarget = null;
    }
}