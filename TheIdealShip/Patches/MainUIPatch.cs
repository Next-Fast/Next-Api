using System;
using HarmonyLib;
using UnityEngine;

namespace TheIdealShip.Patches
{
    [HarmonyPatch]
    public static class MainUIPatch
    {
        [HarmonyPatch(typeof(MainMenuManager),nameof(MainMenuManager.Start)),HarmonyPrefix]
        public static void Start_Prefix(MainMenuManager __instance)
        {
            // 将玩法说明改为Github
            var howtoplayButton = GameObject.Find("HowToPlayButton");
            if (howtoplayButton != null)
            {
                howtoplayButton.GetComponent<PassiveButton>().OnClick = new();
                howtoplayButton.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() => Application.OpenURL("https://github.com/TheIdealShipAU/TheIdealShip")));
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => howtoplayButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText("Github"))));
            }


        }
    }
}