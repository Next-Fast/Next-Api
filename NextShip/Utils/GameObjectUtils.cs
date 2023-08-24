using System;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace NextShip;

public static class GameObjectUtils
{
    public static int GameObjetCount = 0;
    // form TOHE
    /// <summary>
    ///     删除翻译文本（<see cref="TextTranslatorTMP" />）组件
    /// </summary>
    public static void DestroyTranslator(this GameObject obj)
    {
        TextTranslatorTMP[] translatorTMPs = obj.GetComponentsInChildren<TextTranslatorTMP>(true);
        if (translatorTMPs != null) translatorTMPs.Do(n => Object.Destroy(n));
    }

    /// <summary>
    ///     删除翻译文本（<see cref="TextTranslatorTMP" />）组件
    /// </summary>
    public static void DestroyTranslator(this MonoBehaviour obj)
    {
        obj.gameObject.DestroyTranslator();
    }

    /// <summary>
    ///     删除纠正位置（<see cref="AspectPosition" />）组件
    /// </summary>
    public static void DestroyAspectPosition(this GameObject gameObject)
    {
        AspectPosition[] aspectPositions = gameObject.GetComponentsInChildren<AspectPosition>(true);
        if (aspectPositions != null) aspectPositions.Do(n => Object.Destroy(n));
    }

    public static void DestroyComponents<T>(this GameObject gameObject) where T : Object
    {
        T[] components = gameObject.GetComponentsInChildren<T>(true);
        if (components != null) components.Do(n => Object.Destroy(n));
    }

    public static (GameObject, T) CreateGameObjet<T>(string name = null, Transform  parent = null) where T : Component
    {
        var gameObject = CreateGameObject(name, parent);
        return (gameObject, gameObject.AddComponent<T>());
    }
    
    public static PassiveButton CreatePassiveButton
    (
        this GameObject @object,
        string text = "",
        Action onClick = null,
        Color color = default,
        GameObject activeSprite = null,
        GameObject inactiveSprite = null,
        GameObject disabledSprite = null,
        AudioClip ClickSound = null,
        AudioClip HoverSound = null,
        Action[] OnMouseOut = null,
        Action[] OnMouseOver = null
    )
    {
        var button = @object.AddComponent<PassiveButton>();
        
        /*if (text != null) button.buttonText.text = text;
        if (color != default) button.buttonText.color = color;*/
        if (ClickSound) button.ClickSound = ClickSound;
        if (HoverSound) button.HoverSound = HoverSound;
        if (onClick != null) button.AddOnClickListeners(onClick);

        /*if (!@object.GetComponent<SpriteRenderer>()) @object.AddComponent<SpriteRenderer>();
        if (!@object.GetComponent<BoxCollider2D>()) @object.AddComponent<BoxCollider2D>().size = @object.GetComponent<SpriteRenderer>().size;*/
        
        button.OnMouseOut = new UnityEvent();
        button.OnMouseOver = new UnityEvent();

        OnMouseOut?.Do(n => button.OnMouseOut.AddListener(n));
        OnMouseOver?.Do(n => button.OnMouseOver.AddListener(n));        
        
        button.activeSprites = activeSprite;
        button.inactiveSprites = inactiveSprite;
        button.disabledSprites = disabledSprite;
        
        return button;
    }

    public static GameObject CreateGameObject(string name = null, Transform parent = null)
    {
        GameObject gameObject = new GameObject();
        gameObject.name = name ?? $"NextShip_GameObject_{GameObjetCount}";
        if (parent != null) gameObject.transform.SetParent(parent);
        return gameObject;
    }

    public static void AllGameObjectDo(this GameObject gameObject, Action<GameObject> action)
    {
        action(gameObject);
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject obj = gameObject.transform.GetChild(i).gameObject;
            action(obj);
            obj.AllGameObjectDo(action);
        }
    }
}