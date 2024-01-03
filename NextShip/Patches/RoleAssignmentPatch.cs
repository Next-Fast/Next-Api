using AmongUs.GameOptions;
using HarmonyLib;
using NextShip.Api.Attributes;
using NextShip.Manager;
using UnityEngine;

namespace NextShip.Patches;

[Harmony]
internal class GameOptionsDataPatch
{
    [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.GetAdjustedNumImpostors))]
    [HarmonyPostfix]
    public static void GetAdjustedNumImpostorsPatch_Postfix(ref int __result)
    {
        __result = Mathf.Clamp(GameOptionsManager.Instance.CurrentGameOptions.NumImpostors, 1, 3);
    }

    [HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.Validate))]
    [HarmonyPostfix]
    public static void ValidatePatch_Postfix(GameOptionsData __instance)
    {
        __instance.NumImpostors = GameOptionsManager.Instance.CurrentGameOptions.NumImpostors;
    }
}

[Harmony]
internal class RoleManagerPatch
{
    [ServiceAdd] private static readonly NextRoleManager _nextRoleManager;

    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    public static void SelectRoles_Postfix(RoleManager __instance)
    {
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.EndGame))]
    public static void OnGameEnd_Postfix(GameManager __instance)
    {
        NextPlayerManager.Instance.Clear();
        _nextRoleManager.Clear();
    }
}