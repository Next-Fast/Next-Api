using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(LogicGameFlowNormal),nameof(LogicGameFlowNormal.CheckEndCriteria))]
    class CheckGameEndPatch
    {
        public static bool Prefix()
        {
            if (CustomOptionHolder.noGameEnd.getBool()) return false;
            return true;
        }
    }
}