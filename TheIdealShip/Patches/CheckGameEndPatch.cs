using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(LogicGameFlowNormal),nameof(LogicGameFlowNormal.CheckEndCriteria))]
    class CheckGameEndPatch
    {
        public static bool keyse = false;
        public static bool Prefix()
        {
            if (CustomOptionHolder.nogameend.getBool()) return keyse;
            return keyse;
        }
    }
    class KeyCodepatch
    {
        public static void keycodegameend()
        {
            if (GetKeysDown(KeyCode.Return, KeyCode.L, KeyCode.LeftShift))
            {
                CheckGameEndPatch.keyse = true;
                CheckGameEndPatch.Prefix();
                GameManager.Instance.LogicFlow.CheckEndCriteria();
            }
        }

        static bool GetKeysDown(params KeyCode[] keys)
        {
            if (keys.Any(k => Input.GetKeyDown(k)) && keys.All(k => Input.GetKey(k)))
            {
                Helpers.CWrite($"KeyDown:{keys.Where(k => Input.GetKeyDown(k)).First()} in [{string.Join(",", keys)}]");
                return true;
            }
            return false;
        }
    }
}