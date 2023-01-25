using System;
using HarmonyLib;
using TheIdealShip.Utilities;

namespace TheIdealShip.Patches
{
    /* class PlayerPhysicsPatch
    {
        // 运行后修改幽灵移速
        [HarmonyPatch(typeof(PlayerPhysics),nameof(PlayerPhysics.FixedUpdate))]
        public static void Prefix (PlayerPhysics __instance)
        {
            if (CustomOptionHolder.PlayerOption.getBool())
            {
                __instance.GhostSpeed = CustomOptionHolder.PlayerGhostSpeed.getFloat();
                for (var i = 0 ; i < PlayerControl.AllPlayerControls.Count ; i++)
                {
                    PlayerControl.AllPlayerControls[i].MyPhysics.GhostSpeed = CustomOptionHolder.PlayerGhostSpeed.getFloat();
                }
            }
        }
    } */
}