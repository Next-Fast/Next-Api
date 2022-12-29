using HarmonyLib;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(GameStartManager),nameof(GameStartManager.Update))]
    class GameStartManagerPatch
    {
        public static void Prefix(GameStartManager __instance)
        {
            if (CustomOptionHolder.nogameend.getBool())
            {
                __instance.MinPlayers = 1;
                __instance.countDownTimer = 0;
            }
            else
            {
                __instance.MinPlayers = 4;
            }
        }
    }
}