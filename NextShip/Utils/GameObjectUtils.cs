using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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

    public static (GameObject, T) CreateGameObject<T>(string name = null, Transform  parent = null, Vector3 vector3 = default) where T : Component
    {
        var gameObject = CreateGameObject(name, parent, vector3);
        return (gameObject, gameObject.AddComponent<T>());
    }
    
    public static PassiveButton CreatePassiveButton
    (
        this GameObject @object,
        Action onClick = null,
        GameObject activeSprite = null,
        GameObject inactiveSprite = null,
        GameObject disabledSprite = null,
        AudioClip ClickSound = null,
        AudioClip HoverSound = null,
        Action OnMouseOut = null,
        Action OnMouseOver = null
    )
    {
        var button = @object.AddComponent<PassiveButton>();
        
        if (ClickSound) button.ClickSound = ClickSound;
        if (HoverSound) button.HoverSound = HoverSound;
        if (onClick != null) button.OnClick.AddListener(onClick);

        if (!@object.GetComponent<BoxCollider2D>() && @object.GetComponentInChildren<SpriteRenderer>())
        {
            @object.AddComponent<BoxCollider2D>().size = @object.GetComponentInChildren<SpriteRenderer>().size;
        }
        
        button.OnMouseOut = new UnityEvent();
        button.OnMouseOver = new UnityEvent();

        if (OnMouseOut != null)button.OnMouseOut.AddListener(OnMouseOut);
        if (OnMouseOver　!= null) button.OnMouseOver.AddListener(OnMouseOver);
        
        button.activeSprites = activeSprite;
        button.inactiveSprites = inactiveSprite;
        button.disabledSprites = disabledSprite;
        
        return button;
    }

    public static GameObject CreateGameObject(string name = null, Transform parent = null, Vector3 position = default)
    {
        var gameObject = new GameObject
        {
            name = name ?? $"NextShip_GameObject_{GameObjetCount}",
            transform =
            {
                parent = parent,
                localPosition = position == default ? Vector3.zero : position
            }
        };
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
    
    public static bool GetGameObjetWithCondition(out GameObject LastgameObject, out List<Transform> allTransform,out List<bool> bools, bool test = false, params string[] stings)
    {
        bools = new List<bool>();
        allTransform = new List<Transform>();
        LastgameObject = null;
        for (var i = 0; i < stings.Length; i++)
        {

            if (i == 0)
            {
                LastgameObject = GameObject.Find(stings[i]);
                bools.Add(LastgameObject);
                if (LastgameObject)
                {
                    allTransform.Add(LastgameObject.transform);
                }

                if (test)
                {
                    var testObj = GameObject.Find(stings[i]);
                    Info($"test: i,{i} name, string,{stings[i]} bool,{testObj != null}");
                }
            }
            else
            {
                if (!LastgameObject) return false;
                var transform = LastgameObject.transform.Find(stings[i]);
                bools.Add(transform);
                LastgameObject = transform.gameObject;
                allTransform.Add(transform);
                
                if (test)
                {
                    var testObj = GameObject.Find(stings[i]);
                    Info($"test: i,{i} name, string,{stings[i]} bool,{testObj != null}");
                }
            }
            
            Info($"i:{i} name:{stings[i]} bool:{bools.Last()}");
        }
        return bools.Last();
    }

    public static GameObject GetGameObjetFormAll(string name, string[] OptionPaths = null)
    {
        var RootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        
        if (OptionPaths == null)
        {
            return RootGameObjects.FirstOrDefault(n => n.name == name);
        }

        var list = RootGameObjects.Where(gameObjet => gameObjet.name == name).ToList();

        foreach (var obj in list)
        {
            var exist = true;
            var objList = new List<GameObject>();
            objList.AddRange(obj.GetAllChild());
            objList.AddRange(obj.GetAllParent());

            foreach (var path in OptionPaths)
            {
                if (objList.Find(n => n.name == path) == null) exist = false;
            }

            if (exist) return obj;
        }

        return null;
    }

    public static IEnumerable<GameObject> GetAllParent(this GameObject gameObject)
    {
        var transform = gameObject.transform.parent;
        var allObject = new List<GameObject>();
        while (transform)
        {
            allObject.Add(transform.gameObject);
            transform = transform.parent;
        }

        return allObject;
    }

    public static IEnumerable<GameObject> GetAllChild(this GameObject gameObject)
    {
        var count = gameObject.transform.GetChildCount();
        var list = new List<GameObject>();
        
        for (var i = 0; i < count; i++)
        {
            var obj = gameObject.transform.GetChild(i).gameObject;
            list.Add(obj);
            list.AddRange(obj.GetAllChild());
        }

        return list;
    }
}