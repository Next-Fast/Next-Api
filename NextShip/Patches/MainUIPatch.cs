using System;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using NextShip.Manager;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;
using static UnityEngine.UI.Button;


namespace NextShip.Patches;

[HarmonyPatch]
public static class MainUIPatch
{
    public static GameObject Ambience;
    public static GameObject Au_Logo;
    public static GameObject TIS_Logo;
    public static GameObject BackGround;
    private static GameObject LeftPanel;

    private static SpriteRenderer Au_Logo_SpriteRenderer;
    private static SpriteRenderer BackGround_SpriteRenderer;
    private static SpriteRenderer TIS_Logo_SpriteRenderer;

    public static Sprite Au_Logo_Sprite;
    public static Sprite TIS_Logo_Sprite;

    public static bool ChangeStyle;

    private static GameObject Video;

    private static void InitGameObject()
    {
        Ambience = GameObject.Find("Ambience");
        Au_Logo = GameObject.Find("LOGO-AU");
        LeftPanel = GameObject.Find("LeftPanel");
        BackGround = new GameObject("TIS_BackGround");
        TIS_Logo = new GameObject("TIS_Logo");

        Au_Logo_SpriteRenderer = Au_Logo.GetComponent<SpriteRenderer>();
        BackGround_SpriteRenderer = BackGround.AddComponent<SpriteRenderer>();
        TIS_Logo_SpriteRenderer = TIS_Logo.AddComponent<SpriteRenderer>();

        Au_Logo_Sprite = Au_Logo_SpriteRenderer.sprite;
        TIS_Logo_Sprite = SpriteUtils.LoadSpriteFromResources("NextShip.Resources.Logo.Banner.png", 300f);
    }

    private static void Init_SetGameObjectTransform()
    {
        TIS_Logo.transform.SetParent(LeftPanel.transform);
    }

    private static void Init_DestroyGameObject()
    {
    }

    private static void Create()
    {
        var Au_Logo_PassiveButton = Au_Logo.AddComponent<PassiveButton>();
        if (!Au_Logo_PassiveButton) return;
        Au_Logo_PassiveButton.OnClick = new ButtonClickedEvent();
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

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    [HarmonyPrefix]
    public static void MainMenuManager_Start_Prefix_Patch(MainMenuManager __instance)
    {
        InitGameObject();
        Info("初始化选项");
    }
    
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.LateUpdate))]
    [HarmonyPostfix]
    public static void MainMenuManager_Update_Postfix_Patch(MainMenuManager __instance)
    {
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    [HarmonyPostfix]
    public static void MainMenuManager_Start_Postfix_Patch(MainMenuManager __instance)
    {
        __instance.gameObject.SetActive(false);
        if (!Video)
        {
            Video = new GameObject();
            var videoPlayer = Video.AddComponent<VideoPlayer>();
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            videoPlayer.targetCamera = Camera.main;
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = BepInEx.Paths.GameRootPath + "/TIS_Data/TEMP/YuanShen.mp4";
            videoPlayer.Play();
            videoPlayer.loopPointReached += (VideoPlayer.EventHandler)EndWithVideoPlay;

            void EndWithVideoPlay(VideoPlayer source)
            {
                Video.SetActive(false);
                __instance.gameObject.SetActive(true);
            }
        }
    }

    [HarmonyPatch(typeof(AccountManager), nameof(AccountManager.accountTab.Awake))]
    [HarmonyPrefix]
    public static bool AccountManager_OnSceneLoaded_Prefix_Patch(AccountManager __instance)
    {
        return false;
    }
    
}