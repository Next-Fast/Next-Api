using NextShip.Options;
using NextShip.Utilities.Attributes;
using UnityEngine;

namespace NextShip.Roles;

public class Sheriff : RoleBase
{
    public static PlayerControl sheriff;
    public static Color color = new Color32(248, 205, 70, byte.MaxValue);
    public static float shootNumber = 5f;
    public static float cooldown = 30f;
    public static PlayerControl currentTarget;
    
    public static SimpleRoleInfo simpleRoleInfo = new
    (
        typeof(Sheriff),
        RoleId.Sheriff,
        color,
        RoleTeam.Crewmate,
        RoleType.MainRole
    );
    
    public static void clearAndReload()
    {
        sheriff = null;
        currentTarget = null;
        cooldown = sheriffCooldown.getFloat();
        shootNumber = sheriffshootNumber.getFloat();
    }

    public static void Load()
    {
        sheriffSpawnRate = CustomOption.Create(20, Types.Crewmate, cs(color, "Sheriff"), rates, null, true);
        sheriffCooldown =
            CustomOption.Create(21, Types.Crewmate, "SheriffCooldown", 30f, 10f, 60f, 2.5f, sheriffSpawnRate);
        sheriffshootNumber = CustomOption.Create(22, Types.Crewmate, "ShootNumber", 5f, 1f, 15f, 1f, sheriffSpawnRate);
    }

    public Sheriff(PlayerControl player) : base(player)
    {
        SimpleRoleInfo = simpleRoleInfo;
    }

    [OptionLoad]
    public static void OptionLoad()
    {
        RoleOptionBase Sheriff = new RoleOptionBase(simpleRoleInfo.name, -1, optionTab.Crewmate);
    }
}