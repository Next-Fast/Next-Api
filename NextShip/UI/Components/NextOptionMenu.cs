using System;
using System.Linq;
using Il2CppInterop.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace NextShip.UI.Components;

public class NextOptionMenu
{
    public bool Initd;
    public OptionsConsole __instance;
    private readonly GameObject  NextMenuParent;
    private GameObject NextMenu;

    public NextOptionMenu(OptionsConsole __instance, GameObject NextMenuParent)
    {
        Initd = false;
        
        this.__instance = __instance;
        this.NextMenuParent = NextMenuParent;
    }
    
    public void Init()
    {
        if (Initd) return;
        
        var tint = Object.Instantiate(__instance.MenuPrefab.transform.Find("Tint"), NextMenuParent.transform);
        var background = Object.Instantiate(__instance.MenuPrefab.transform.Find("Background"), NextMenuParent.transform);
        
            
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
        
        var scrollbar = ScrollBar_Handle.GetComponent<Scrollbar>();
        scrollbar.parent = scroller;
        scrollbar.trackGraphic = ScrollBar_Track.GetComponent<SpriteRenderer>();

        var button = CreateButton("abab", "abab", "button", new Vector3(0, 0, 0));
        button.transform.SetParent(list.transform);
            
        
        NextMenuParent.SetActive(false);
        NextMenuParent.AllGameObjectDo(n => n.layer = tint.gameObject.layer);
        
        Initd = true;
    }
    
    private GameObject CreateButton(string Title, string text, string name, Vector3 LocalPosition, Action action = null)
    {

        var button = new GameObject(name)
        {
            transform =
            {
                localPosition = LocalPosition
            }
        };
        button.transform.SetParent(NextMenuParent.transform);
        button.CreatePassiveButton(onClick:action);
            
        var backGround = new GameObject("BackGround");
        backGround.transform.SetParent(button.transform);
        backGround.transform.localPosition = new Vector3(0, 0, -1);
            
        var backGroundSprite = backGround.AddComponent<SpriteRenderer>();
        backGroundSprite.sprite = ObjetUtils.Find<Sprite>("buttonClick");
        backGroundSprite.drawMode = SpriteDrawMode.Sliced;
        backGroundSprite.size = new Vector2(2.5f, 1.3f);
            
        button.AddComponent<BoxCollider2D>().size = backGroundSprite.size;

        var titleTextGameObject = new GameObject("TitleText");
        titleTextGameObject.transform.SetParent(button.transform);
        titleTextGameObject.transform.localPosition = new Vector3(9.4f, -2.2f, 0);
            
        var textMeshPro = titleTextGameObject.AddComponent<TextMeshPro>();
        textMeshPro.text = Title;
        textMeshPro.fontSize = 5; 
            
        var SubTextGameObject = Object.Instantiate(titleTextGameObject, button.transform);
        SubTextGameObject.name = "SubText";
        SubTextGameObject.transform.localPosition = new Vector3(10.4F, -2.8f, 0);
            
        var SubTextMeshPro = SubTextGameObject.GetComponent<TextMeshPro>();
        SubTextMeshPro.text = text;
        SubTextMeshPro.fontSize = 3;
            
        return button;
    }
}