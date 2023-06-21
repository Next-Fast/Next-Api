using HarmonyLib;
using UnityEngine;

namespace TheIdealShip.Patches
{
    [HarmonyPatch]
    public static class MainUIPatch
    {
        public static GameObject Ambience;
        public static GameObject Au_Logo;

        public static void InitGameObject()
        {
            Ambience = GameObject.Find("Ambience");
            Au_Logo = GameObject.Find("LOGO-AU");
        }

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPrefix]
        public static void MainMenuManager_Start_Prefix_Patch(MainMenuManager __instance)
        {
            InitGameObject();
        }

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPostfix]
        public static void MainMenuManager_Start_Postfix_Patch(MainMenuManager __instance)
        {
        }

        [HarmonyPatch(typeof(AccountManager), nameof(AccountManager.OnSceneLoaded)), HarmonyPrefix]
        public static bool AccountManager_OnSceneLoaded_Prefix_Patch()
        {
            return false;
        }
    }
}