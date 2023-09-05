using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using NextShip.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace NextShip.UI.Patches;

[HarmonyPatch]
public static class MainUIPatch
{
    private static GameObject Ambience;
    private static GameObject Au_Logo;
    private static GameObject TIS_Logo;
    private static GameObject BackGround;
    private static GameObject LeftPanel;
    private static GameObject RightPanel;
    private static GameObject BackgroundTexture;
    private static GameObject Main_Button;

    private static SpriteRenderer BackGround_SpriteRenderer;
    private static SpriteRenderer TIS_Logo_SpriteRenderer;
    private static SpriteRenderer Au_Logo_SpriteRenderer;

    public static Sprite TIS_Logo_Sprite;
    private static Sprite NextShipText_Sprite;
    private static Sprite Au_Logo_Sprite;
    private static Sprite TOHE_Sprite;
    private static Sprite NextShipSprite;

    public static bool ChangeStyle;
    private static bool RightPanelIsOpen;

    public static int time = 0;



    private static void InitGameObject()
    {
        BackGround = new GameObject("TIS_BackGround");
        TIS_Logo = new GameObject("TIS_Logo");

        Ambience = GameObject.Find("Ambience");
        Au_Logo = GameObject.Find("LOGO-AU");
        LeftPanel = GameObject.Find("LeftPanel");
        RightPanel = GameObject.Find("RightPanel");
        BackgroundTexture = GameObject.Find("BackgroundTexture");

        BackGround_SpriteRenderer = BackGround.AddComponent<SpriteRenderer>();
        TIS_Logo_SpriteRenderer = TIS_Logo.AddComponent<SpriteRenderer>();
        Au_Logo_SpriteRenderer = Au_Logo.GetComponent<SpriteRenderer>();

        Au_Logo_Sprite = Au_Logo_SpriteRenderer.sprite;

        TIS_Logo_Sprite = SpriteUtils.LoadSpriteFromResources("NextShip.Resources.Logo.Banner.png", 300f);
        NextShipText_Sprite = SpriteUtils.LoadSpriteFromResources("NextShip.Resources.Logo.NextShipText.png", 100f);
        TOHE_Sprite = SpriteUtils.LoadSpriteFromResources("NextShip.Resources.Logo.TOHE-BG.jpg", 179f);
        NextShipSprite = SpriteUtils.LoadSpriteFromResources("NextShip.Resources.Logo.NextShip.png", 180f);
    }

    private static void Init_DestroyGameObject()
    {
        LeftPanel.DestroyComponents<AspectSize>();
        LeftPanel.DestroyComponents<AspectPosition>();
        RightPanel.DestroyComponents<AspectPosition>();
    }
    
    private static void Create(MainMenuManager __instance)
    {
        Au_Logo.AddComponent<BoxCollider2D>().size = Au_Logo_SpriteRenderer.size;

        var auLogoPassiveButton = Au_Logo.AddComponent<PassiveButton>();
        
        auLogoPassiveButton.OnClick.AddListener((UnityAction)Au_Logo_OnClick);
        auLogoPassiveButton.OnMouseOut = new UnityEvent();
        auLogoPassiveButton.OnMouseOver = new UnityEvent();

        /*
        var starObject = Object.Instantiate(Ambience.transform.Find("starfield"), BackGround.transform);
        var star = starObject.GetComponent<StarGen>();
        star.SetDirection(new Vector2(0, -1.5f));

        Main_Button = new GameObject("Main_Button");
        Main_Button.transform.SetParent(GameObject.Find("MainUI").transform);
        
        var standardActiveSprite = __instance.newsButton.activeSprites.GetComponent<SpriteRenderer>().sprite;
        var minorActiveSprite = __instance.quitButton.activeSprites.GetComponent<SpriteRenderer>().sprite;
        
        List<(List<PassiveButton>, Color)> mainButtons = new()
        {
            (new List<PassiveButton> {__instance.playButton, __instance.inventoryButton, __instance.shopButton}, new Color(1f, 0.524f, 0.549f, 0.8f)),
            (new List<PassiveButton> {__instance.newsButton, __instance.myAccountButton, __instance.settingsButton}, new Color(1f, 0.825f, 0.686f, 0.8f)),
            (new List<PassiveButton> {__instance.creditsButton, __instance.quitButton}, new Color(0.526f, 1f, 0.792f, 0.8f)),
        };
        
        mainButtons.Do(CreateButton);
        Main_Button.SetActive(false);
        */

        TIS_Logo.transform.position = new Vector3(2f, -0.2f, 0);
        TIS_Logo.transform.localScale = new Vector3(1.1f, 1.5f, 1);
        TIS_Logo_SpriteRenderer.sprite = TIS_Logo_Sprite;
        

        /*BackGround_SpriteRenderer.sprite = TOHE_Sprite;
        BackGround.SetActive(false);*/

        /*void CreateButton((List<PassiveButton> IButton, Color color) b)
        {
            foreach (var IButton in b.IButton)
            {
                var color = b.color;
                var button = Object.Instantiate(IButton.gameObject, Main_Button.transform).GetComponent<PassiveButton>();
                button.gameObject.DestroyComponents<AspectPosition>();
                button.activeSprites.transform.FindChild("Shine")?.gameObject.SetActive(false);
                button.inactiveSprites.transform.FindChild("Shine")?.gameObject.SetActive(false);
                var activeRenderer = button.activeSprites.GetComponent<SpriteRenderer>();
                var inActiveRenderer = button.inactiveSprites.GetComponent<SpriteRenderer>();
                activeRenderer.sprite = minorActiveSprite;
                inActiveRenderer.sprite = minorActiveSprite;
                activeRenderer.color = new Color(color.r, color.g, color.b, 1f);
                inActiveRenderer.color = color;
                button.activeTextColor = Color.white;
                button.inactiveTextColor = Color.white;
            }
        }*/
    }

    private static void Au_Logo_OnClick()
    {
        ChangeStyle = !ChangeStyle;
        UpdateMainUI();
    }

    private static void UpdateMainUI()
    {
        TIS_Logo.SetActive(!ChangeStyle);
        /*Ambience.SetActive(!ChangeStyle);
        LeftPanel.SetActive(!ChangeStyle);
        Main_Button.SetActive(ChangeStyle);
        BackGround.gameObject.SetActive(ChangeStyle);*/

        Au_Logo_SpriteRenderer.sprite = ChangeStyle ? NextShipText_Sprite : Au_Logo_Sprite;
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    [HarmonyPostfix]
    public static void MainMenuManager_Start_Postfix_Patch(MainMenuManager __instance)
    {
        try
        {
            InitGameObject();
            Init_DestroyGameObject();
            
            // 创建主菜单
            Create(__instance);
            Info("创建主界面");
        }
        catch (Exception e)
        {
            Exception(e);
        }
    }

    /*[HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.LateUpdate))]
    [HarmonyPostfix]
    public static void MainMenuManager_LateUpdate_Postfix_Patch(MainMenuManager __instance)
    {
        if (!ChangeStyle) return;

        time++;

        if (time <= 3) return;
        var random = new Random();
        var count = random.Next(0, 1);

        BackGround_SpriteRenderer.sprite = count > 0 ? TOHE_Sprite : NextShipSprite;

        time = 0;
    }*/

    [
        HarmonyPatch(typeof(MainMenuManager)),
        HarmonyPatch(nameof(MainMenuManager.OpenGameModeMenu)),
        HarmonyPatch(nameof(MainMenuManager.OpenAccountMenu)),
        HarmonyPatch(nameof(MainMenuManager.OpenCredits)),
        HarmonyPostfix
    ]
    public static void OpenMenuPostfix()
    {
        if (TIS_Logo) TIS_Logo.SetActive(false);
    }
    

    [
        HarmonyPatch(typeof(MainMenuManager)),
        HarmonyPatch(nameof(MainMenuManager.ResetScreen)), 
        HarmonyPostfix
    ]
    public static void ResetScreenPostfix()
    {
        if (TIS_Logo) TIS_Logo.SetActive(false);
    }
}