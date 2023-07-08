using System.Text.RegularExpressions;
using HarmonyLib;
using UnityEngine;

namespace TheIdealShip.Patches;

[HarmonyPatch(typeof(JoinGameButton), nameof(JoinGameButton.OnClick))]
internal class JoinGameButtonPatch
{
    public static void Prefix(JoinGameButton __instance)
    {
        if (__instance.GameIdText == null) return;
        if (__instance.GameIdText.text == "" &&
            Regex.IsMatch(GUIUtility.systemCopyBuffer.Trim('\r', '\n'), @"^[a-zA-Z]{6}$"))
            __instance.GameIdText.SetText(GUIUtility.systemCopyBuffer.Trim('\r', '\n'));
    }
}