using UnityEngine;
using static TheIdealShip.Roles.RoleInfo;

namespace TheIdealShip.Roles;
public class SchrodingersCat
{
    public static PlayerControl schrodingersCat;
    public static Color color = new Color(130, 130, 130);
    public static RoleTeam team = RoleTeam.Neutral;

    public static void clearAndReload()
    {
        schrodingersCat = null;
        team = RoleTeam.Neutral;
    }

    public static void OptionLoad()
    {
        SchrodingersCatRate = CustomOption.Create(161, Types.Neutral, cs(SchrodingersCat.color, "Schrodinger's Cat"), rates, null, true);
    }

}
