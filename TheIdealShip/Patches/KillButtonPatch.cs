using HarmonyLib;
using TheIdealShip.Roles;

namespace TheIdealShip.Patches;

internal class KillButtonPatch
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class PlayerControlMurderPlayerPatch
    {
        public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            if (Lover.lover1 != null && Lover.lover2 != null && (target == Lover.lover1 || target == Lover.lover2) &&
                LoverDieForLove.getBool() && !Lover.suicide)
            {
                Lover.suicide = true;
                if (target == Lover.lover1) Lover.lover2.Suicide();

                if (target == Lover.lover2) Lover.lover1.Suicide();
            }

/*                 if (SchrodingersCat.schrodingersCat != null && target == SchrodingersCat.schrodingersCat)
                {
                    RoleInfo.RoleTeam STeam = RoleHelpers.GetRoleInfo(__instance).team;
                    RPCHelpers.Create((byte)CustomRPC.SchrodingerSCatTeamChange, bytes: new []{ (byte)STeam });
                    return false;
                }
 */
            return true;
        }
    }
}