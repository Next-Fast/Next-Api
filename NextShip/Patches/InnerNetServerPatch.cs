/* using HarmonyLib;

namespace NextShip.Patches
{
    [HarmonyPatch(typeof(InnerNet.InnerNetServer), nameof(InnerNet.InnerNetServer.KickPlayer))]
    class InnerNetServerPatch
    {
        public static bool Prefix()
        {
            if (CustomOptionHolder.disableServerKickPlayer.getBool())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
} */

