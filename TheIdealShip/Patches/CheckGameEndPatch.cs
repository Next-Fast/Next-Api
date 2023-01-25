using System.Linq;
using HarmonyLib;
using TheIdealShip.Roles;
using UnityEngine;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(LogicGameFlowNormal),nameof(LogicGameFlowNormal.CheckEndCriteria))]
    class CheckGameEndPatch
    {
        public static bool Prefix(ShipStatus __instance)
        {
            if (CustomOptionHolder.noGameEnd.getBool()) return false;
            if (!GameData.Instance) return false;
            if (CheckAndEndGameForJesterWin(__instance)) return false;
            return true;
        }

        private static bool CheckAndEndGameForJesterWin(ShipStatus __instance)
        {
            if (Jester.triggerJesterWin)
            {
                //__instance.enabled = false;
                GameManager.Instance.RpcEndGame((GameOverReason)CustomGameOverReason.JesterWin, false);
                return true;
            }
            return false;
        }
    }
}