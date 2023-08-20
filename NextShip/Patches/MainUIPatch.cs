using System;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using NextShip.Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Video;
using static UnityEngine.UI.Button;
using Object = UnityEngine.Object;


namespace NextShip.Patches;

[HarmonyPatch]
public static class MainUIPatch
{
    private static GameObject Ambience;
    private static GameObject Au_Logo;
    private static GameObject TIS_Logo;
    private static GameObject BackGround;
    private static GameObject LeftPanel;
    
    private static SpriteRenderer BackGround_SpriteRenderer;
    private static SpriteRenderer TIS_Logo_SpriteRenderer;
    private static SpriteRenderer Au_Logo_SpriteRenderer;
    
    public static Sprite TIS_Logo_Sprite;
    public static Sprite NextShipText_Sprite;
    public static Sprite Au_Logo_Sprite;
    public static Sprite TOHE_Sprite;
    
    public static bool ChangeStyle;
    

    private static void InitGameObject()
    {
        BackGround = new GameObject("TIS_BackGround");
        TIS_Logo = new GameObject("TIS_Logo");
        
        Ambience = GameObject.Find("Ambience");
        Au_Logo = GameObject.Find("LOGO-AU");
        LeftPanel = GameObject.Find("LeftPanel");
        
        BackGround_SpriteRenderer = BackGround.AddComponent<SpriteRenderer>();
        TIS_Logo_SpriteRenderer = TIS_Logo.AddComponent<SpriteRenderer>();
        Au_Logo_SpriteRenderer = Au_Logo.GetComponent<SpriteRenderer>();

        Au_Logo_Sprite = Au_Logo_SpriteRenderer.sprite;
        
        TIS_Logo_Sprite = SpriteUtils.LoadSpriteFromResources("NextShip.Resources.Logo.Banner.png", 300f);
        NextShipText_Sprite = SpriteUtils.LoadSpriteFromResources("NextShip.Resources.Logo.NextShipText.png", 100f);
        TOHE_Sprite = SpriteUtils.LoadSpriteFromResources("NextShip.Resources.Logo.TOHE-BG.jpg", 179f);
    }

    private static void Init_SetGameObjectTransform()
    {
    }

    private static void Init_DestroyGameObject()
    {
    }

    private static void Create()
    {
        InitGameObject();
        
        Au_Logo.AddComponent<BoxCollider2D>().size = Au_Logo_SpriteRenderer.size;
        
        LeftPanel.DestroyComponents<AspectSize>();
            
        var auLogoPassiveButton = Au_Logo.AddComponent<PassiveButton>(); ;
        auLogoPassiveButton.OnClick.AddListener((UnityAction)Au_Logo_OnClick);
        auLogoPassiveButton.OnMouseOut = new UnityEvent();
        auLogoPassiveButton.OnMouseOut.AddListener((UnityAction)(() => Info("Out")));
        auLogoPassiveButton.OnMouseOver = new UnityEvent();
        auLogoPassiveButton.OnMouseOver.AddListener((UnityAction)(() => Info("Over")));

        

        TIS_Logo.transform.position = new Vector3(2f, -0.2f, 0);
        TIS_Logo.transform.localScale = new Vector3(1.1f, 1.5f, 1);
        TIS_Logo_SpriteRenderer.sprite = TIS_Logo_Sprite;

        BackGround_SpriteRenderer.sprite = TOHE_Sprite;
        BackGround.SetActive(false);
    }

    private static void Au_Logo_OnClick()
    {
        ChangeStyle = !ChangeStyle;
        UpdateMainUI();
    }

    public static void UpdateMainUI()
    {
        TIS_Logo.SetActive(!ChangeStyle);
        BackGround.gameObject.SetActive(ChangeStyle);
        
        Au_Logo_SpriteRenderer.sprite = ChangeStyle ? NextShipText_Sprite : Au_Logo_Sprite;
    }
    
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    [HarmonyPostfix]
    public static void MainMenuManager_Start_Postfix_Patch(MainMenuManager __instance)
    {
        try
        {
            // 创建主菜单
            Create();
            Info("创建主界面");
        }
        catch (Exception e)
        {
            Exception(e);
        }
    }
    
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.LateUpdate))]
    [HarmonyPostfix]
    public static void MainMenuManager_Update_Postfix_Patch(MainMenuManager __instance)
    {

    }
    
    
}