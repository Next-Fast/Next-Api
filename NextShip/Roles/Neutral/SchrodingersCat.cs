using UnityEngine;

namespace NextShip.Roles;

public class SchrodingersCat
{
    public static PlayerControl schrodingersCat;
    public static Color color = new(130, 130, 130);
    public static RoleTeam team = RoleTeam.Neutral;

    public static void clearAndReload()
    {
        schrodingersCat = null;
        team = RoleTeam.Neutral;
    }

    public static void OptionLoad()
    {
        SchrodingersCatRate =
            CustomOption.Create(161, Types.Neutral, cs(color, "Schrodinger's Cat"), rates, null, true);
    }
}