using HarmonyLib;
using TheIdealShip.Roles;
using TheIdealShip.Modules;
using static TheIdealShip.Languages.Language;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(ExileController),nameof(ExileController.Begin))]
    class ExileControllerBeginPatch
    {
/*         public static void Postfix(ExileController __instance, [HarmonyArgument(0)] ref GameData.PlayerInfo exiled, [HarmonyArgument(1)] bool tie)
        {
            if (exiled != null)
            {
                var player = exiled.GetPlayerForExile();
/*                 var info = RoleHelpers.GetRoleInfo(player);
                var eText = "\n" + string.Format(GetString("exileText"),exiled.PlayerName,info.name);
                if (CustomOptionHolder.showExilePlayerConcreteRoleTeam.getBool())
                {
                    eText += "\n" + string.Format(GetString("exileTeamText"),RoleHelpers.GetRoleTeam(player));
                }
                if (__instance.ImpostorText.text != null)
                {
                    eText = eText + "\n" + __instance.ImpostorText.text;
                    __instance.ImpostorText.text = "";
                }
                if (CustomOptionHolder.showExilePlayerRole.getBool() && GameManager.Instance.LogicOptions.GetConfirmImpostor())
                {
                    if (info != null)
                    {
                        if (__instance.completeString != "TestPlayer was not The Impostor")
                        {
                            __instance.completeString += eText;
                        }
                    }
                }
            }
        } */
    }

    [HarmonyPatch]
    class ExileControllerWrapUp
    {
        [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
        class ExileControllerWrapUpPatch
        {
            public static void Postfix(ExileController __instance)
            {
                if (__instance.exiled != null)
                {
/*                     if(__instance.exiled.GetPlayerForExile().Is(RoleId.Jester))
                    {
                        Jester.triggerJesterWin = true;
                    } */
                }

                CustomButton.MeetingEndedUpdate();
            }
        }
    }
}