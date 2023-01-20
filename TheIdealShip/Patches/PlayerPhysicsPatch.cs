using System;
using HarmonyLib;

namespace TheIdealShip.Patches
{
    class PlayerPhysicsPatch
    {
        // 运行后修改幽灵移速
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerPhysics))]
        public static void GhostSpeedPatch (PlayerPhysics __instance)
        {
            if (CustomOptionHolder.PlayerOption.getBool() && CustomOptionHolder.PlayerGhostSpeed.getFloat() != __instance.GhostSpeed) __instance.GhostSpeed = CustomOptionHolder.PlayerGhostSpeed.getFloat();
        }
    }
}