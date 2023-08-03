using HarmonyLib;
using NextShip.Utilities;

namespace NextShip.Patches;

[HarmonyPatch(typeof(EmergencyMinigame), nameof(EmergencyMinigame.Update))]
internal class EmergencyMinigameUpdatePatch
{
    public static void Postfix(EmergencyMinigame __instance)
    {
        var roleCanCallEmergency = true;
        var statusText = "";

        var player = CachedPlayer.LocalPlayer.PlayerControl;
/*             var info = RoleHelpers.GetRoleInfo(player); */
/*             var id = info.roleId;

            if (id == RoleId.Jester)
            {
                roleCanCallEmergency = Jester.CanCallEmergency;
                if (!roleCanCallEmergency)
                {
                    statusText = "小丑达咩拍灯";
                }
            } */

        if (!roleCanCallEmergency)
        {
            __instance.StatusText.text = statusText;
            __instance.NumberText.text = string.Empty;
            __instance.ClosedLid.gameObject.SetActive(true);
            __instance.OpenLid.gameObject.SetActive(false);
            __instance.ButtonActive = false;
            __instance.enabled = false;
        }
    }
}