using System.Security.Cryptography.X509Certificates;
using System;
using HarmonyLib;
using UnityEngine;
using static UnityEngine.UI.Button;
using UnityEngine.Video;


namespace TheIdealShip.Patches
{
    [HarmonyPatch]
    public static class MainUIPatch
    {
        public static GameObject Ambience;
        public static GameObject Au_Logo;
        public static GameObject TIS_Logo;
        public static GameObject BackGround;

        private static SpriteRenderer Au_Logo_SpriteRenderer;
        private static SpriteRenderer BackGround_SpriteRenderer;
        private static SpriteRenderer TIS_Logo_SpriteRenderer;

        public static Sprite Au_Logo_Sprite;
        public static Sprite TIS_Logo_Sprite;

        public static VideoPlayer BackGround_Video;

        public static bool ChangeStyle = false;

        private static void InitGameObject()
        {
            Ambience = GameObject.Find("Ambience");
            Au_Logo = GameObject.Find("LOGO-AU");
            BackGround = new GameObject("TIS_BackGround");
            TIS_Logo = new GameObject("TIS_Logo");

            Au_Logo_SpriteRenderer = Au_Logo.GetComponent<SpriteRenderer>();
            BackGround_SpriteRenderer = BackGround.AddComponent<SpriteRenderer>();
            TIS_Logo_SpriteRenderer = TIS_Logo.AddComponent<SpriteRenderer>();

            Au_Logo_Sprite = Au_Logo_SpriteRenderer.sprite;
            TIS_Logo_Sprite = Helpers.LoadSpriteFromResources("TheIdealShip.Resources.Banner.png", 300f);

            BackGround_Video = BackGround.AddComponent<VideoPlayer>();
        }

        private static void Create()
        {
            var Au_Logo_PassiveButton = Au_Logo.GetComponent<PassiveButton>();
            Au_Logo_PassiveButton.OnClick = new ();
            Au_Logo_PassiveButton.OnClick.AddListener((Action)Au_Logo_OnClick);

            TIS_Logo.transform.position = new Vector3(2f, -0.2f, 0);
            TIS_Logo.transform.localScale = new Vector3(1.1f, 1.5f, 1);
            TIS_Logo_SpriteRenderer.sprite = TIS_Logo_Sprite;
        }

        private static void Au_Logo_OnClick()
        {
            ChangeStyle = !ChangeStyle;
            UpdateMainUI();
        }

        public static void UpdateMainUI()
        {
            Ambience.gameObject.SetActive(!ChangeStyle);
            Au_Logo_SpriteRenderer.sprite = ChangeStyle ? TIS_Logo_Sprite : Au_Logo_Sprite;
            BackGround.gameObject.SetActive(ChangeStyle);
            TIS_Logo.SetActive(!ChangeStyle);
        }

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPrefix]
        public static void MainMenuManager_Start_Prefix_Patch(MainMenuManager __instance)
        {
            InitGameObject();
        }

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPostfix]
        public static void MainMenuManager_Start_Postfix_Patch(MainMenuManager __instance)
        {
            Create();
        }

        [HarmonyPatch(typeof(AccountManager), nameof(AccountManager.OnSceneLoaded)), HarmonyPrefix]
        public static bool AccountManager_OnSceneLoaded_Prefix_Patch()
        {
            return false;
        }
    }
}