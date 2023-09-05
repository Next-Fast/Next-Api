using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace NextShip.UI;

public class NextOptionMenu
{
    public bool Initd;
    public OptionsConsole __instance;
    private readonly GameObject  NextMenuParent;
    private GameObject NextMenu;
    private List<GameObject> AllListButton = new ();

    public NextOptionMenu(OptionsConsole __instance, GameObject NextMenuParent)
    {
        Initd = false;
        
        this.__instance = __instance;
        this.NextMenuParent = NextMenuParent;
    }
    
    public void Init()
    {
        if (Initd) return;

        var BackGround = new GameObject("BackGround");
        BackGround.transform.SetParent(NextMenuParent.transform);
        
        var tint = Object.Instantiate(__instance.MenuPrefab.transform.Find("Tint").gameObject, BackGround.transform);
        Object.Instantiate(__instance.MenuPrefab.transform.Find("Background").transform.GetChild(1).gameObject, BackGround.transform);
        
            
        NextMenu = new GameObject("NextMenu");
        NextMenu.transform.SetParent(NextMenuParent.transform);
        
        var NextMenuScroll = new GameObject("NextMenuScroll");
        NextMenuScroll.transform.SetParent(NextMenu.transform);
        

        var Scroll = GameObject.Find("ChatUi").transform.Find("QuickChatMenu").transform
            .Find("Container").transform.Find("Pages").transform.Find("PhrasesPage").transform.Find("Scroller");

        var tem_Handle = Scroll.transform.Find("ScrollBar_Handle");
        var tem_Track = Scroll.transform.Find("ScrollBar_Track");
        
        var ScrollBar_Handle = Object.Instantiate(tem_Handle, NextMenuScroll.transform);
        var ScrollBar_Track = Object.Instantiate(tem_Track, NextMenuScroll.transform);
        ScrollBar_Track.transform.localPosition = new Vector3(3.1f, 3, -1);
        ScrollBar_Track.GetComponent<SpriteRenderer>().size = new Vector2(0.03f, 6);

        var list = new GameObject("List");
        list.transform.SetParent(NextMenu.transform);
        
        var box = NextMenu.AddComponent<BoxCollider2D>();
        box.size = new Vector2(1, 1);
        var scroller = NextMenu.AddComponent<Scroller>();
        scroller.allowX = false;
        scroller.showX = false;
        scroller.allowY = true;
        scroller.showY = true;
        scroller.OnMouseOut = new UnityEvent();
        scroller.OnMouseOver = new UnityEvent();
        scroller.Inner = list.transform;
        scroller.ScrollbarY = ScrollBar_Handle.GetComponent<Scrollbar>();
        scroller.ScrollbarYBounds = new FloatRange(1, 1);
        scroller.ContentYBounds = new FloatRange(1, 1);

        CreateLargeButton("GeneralSettingOption", NextMenuParent.transform, new Vector3(0, 0, 0), "常规设置", "关于设置的选项", () => OpenMenu(MenuIndex.GeneralSetting));
        CreateLargeButton("RoleSettingOption", NextMenuParent.transform, new Vector3(0, 0, 0), "职业设置", "关于职业的选项设置", () => OpenMenu(MenuIndex.RoleSetting));
        CreateSmallButton("CloneButton", NextMenuParent.transform, new Vector3(0, 0, 0), "关闭", Close);
        
        var scrollbar = ScrollBar_Handle.GetComponent<Scrollbar>();
        scrollbar.parent = scroller;
        scrollbar.trackGraphic = ScrollBar_Track.GetComponent<SpriteRenderer>();
        
        NextMenuParent.SetActive(false);
        NextMenuParent.AllGameObjectDo(n => n.layer = tint.gameObject.layer);
        
        Initd = true;
    }

    private void OpenMenu(MenuIndex menu)
    {
        
    }

    private void Close()
    {
        
    }

    private static GameObject CreateSmallButton(string name, Transform Parent, Vector3 position, string Text, Action action = null) =>
        CreateButton(Text, name, Parent, 
            new Vector2(2.5f, 1.3f), 
            action,
            position,
            new Vector3(9.4f, -2.2f, 0), 
            new Vector3(10.4F, -2.8f, 0));

    private static GameObject CreateLargeButton(string name, Transform Parent, Vector3 position, string TitleText, string SubText, Action action = null) =>
        CreateButton(TitleText, name, Parent, 
            new Vector2(2.5f, 1.3f),
            action,
            position,
            new Vector3(9.4f, -2.2f, 0),
            new Vector3(10.4F, -2.8f, 0), 
            true, SubText);

    private GameObject CreateScrollMenu()
    {
        return null;
    }
    
    private static GameObject CreateButton
    (
        string Title,
        string name, 
        Transform Parent,
        Vector2 BackSpriteSize,
        Action action,
        Vector3 LocalPosition,
        Vector3 TitlePosition,
        Vector3 SubTextPosition,
        bool enableSubText = false,
        string Subtext = "", 
        int TitleTextSize = 5,
        int SubTextSize = 3,
        Sprite ButtonBackSprite = null
    )
    {

        var button = new GameObject(name)
        {
            transform =
            {
                localPosition = LocalPosition
            }
        };
        button.transform.SetParent(Parent);
        
        var backGround = new GameObject("BackGround");
        backGround.transform.SetParent(button.transform);
        backGround.transform.localPosition = new Vector3(0, 0, -1);
        
        var backGroundSprite = backGround.AddComponent<SpriteRenderer>();
        backGroundSprite.sprite = ButtonBackSprite ? ButtonBackSprite : ObjetUtils.Find<Sprite>("buttonClick");
        backGroundSprite.drawMode = SpriteDrawMode.Sliced;
        backGroundSprite.size = BackSpriteSize;
        
        button.CreatePassiveButton(action);

        var titleTextGameObject = new GameObject("TitleText");
        titleTextGameObject.transform.SetParent(button.transform);
        titleTextGameObject.transform.localPosition = TitlePosition;
            
        var textMeshPro = titleTextGameObject.AddComponent<TextMeshPro>();
        textMeshPro.text = Title;
        textMeshPro.fontSize = TitleTextSize;

        if (!enableSubText) return button;
        var SubTextGameObject = Object.Instantiate(titleTextGameObject, button.transform);
        SubTextGameObject.name = "SubText";
        SubTextGameObject.transform.localPosition = SubTextPosition;

        var SubTextMeshPro = SubTextGameObject.GetComponent<TextMeshPro>();
        SubTextMeshPro.text = Subtext;
        SubTextMeshPro.fontSize = SubTextSize;

        return button;
    }
    
    private enum MenuIndex
    {
        GeneralSetting = 0,
        RoleSetting = 1,
    }
}