using HarmonyLib;
using TheIdealShip.Roles;
using static TheIdealShip.Languages.Language;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(ExileController),nameof(ExileController.Begin))]
    class ExileControllerBeginPatch
    {
        public static void Postfix(ExileController __instance, [HarmonyArgument(0)] ref GameData.PlayerInfo exiled, [HarmonyArgument(1)] bool tie)
        {
            var info = RoleInfo.GetRoleInfo(Helpers.GetPlayerForId(exiled.PlayerId));
            var eText = "\n" + string.Format(GetString("exileText"),exiled.PlayerName,info.name);
            if (CustomOptionHolder.showExilePlayerRole.getBool() && GameManager.Instance.LogicOptions.GetConfirmImpostor())
            {
                if (info != null)
                {
                    if (__instance.ImpostorText.text != null)
                    {
                        __instance.ImpostorText.text += eText;
                    }
                    if (__instance.completeString != "TestPlayer was not The Impostor")
                    {
                        __instance.completeString += eText;
                    }
                }
            }
        }
    }
}