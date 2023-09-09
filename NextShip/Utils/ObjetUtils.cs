using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using NextShip.Utilities;
using UnityEngine;

namespace NextShip.Utils;

public static class ObjetUtils
{
    public static List<Object> AllObjects = new ();

    public static T Find<T>(string name) where T  : Il2CppObjectBase
    {
        var cacheObj = AllObjects.FirstOrDefault(n => n.name == name);
        if (cacheObj != null)
        {
            Info($"Cache: {name} type {nameof(T)}");
            return cacheObj.CastFast<T>();
        }
        
        var find = false;
        Object GetObject = null;
        
        foreach (var Obj in Resources.FindObjectsOfTypeAll(Il2CppType.Of<T>()))
        {
            if (Obj.name != name) continue;
            find = true;
            GetObject = Obj;
            
            GetObject.DontDestroyAndUnload();
            AllObjects.Add(GetObject);
        }
        
        
        Info($"ObjectUtils.Find return isnull:{find} Find<{typeof(T).Name}> Get:{name}");
        return find ? GetObject.CastFast<T>() : null ;
    }
    
    /// <summary>
    /// 使Object 加载时候不摧毁
    /// </summary>
    /// <param name="objects"></param>
    public static void Do(params Object[] objects)
    {
        objects.Do(n =>
        {
            AllObjects.Add(n);
            Object.DontDestroyOnLoad(n);
            Info($"DontDestroyOnLoad : {n.name}");
        });
    }

    public static T DontDestroyAndUnload<T>(this T obj) where T : Object
    {
        obj.hideFlags |= HideFlags.DontUnloadUnusedAsset;
        obj.hideFlags |= HideFlags.HideAndDontSave;
        return obj;
    }
    
    // form Reactor
    
    /// <summary>
    /// Stops <paramref name="obj"/> from being destroyed.
    /// 不销毁
    /// </summary>
    /// <param name="obj">The object to stop from being destroyed.</param>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>Passed <paramref name="obj"/>.</returns>
    public static T DontDestroy<T>(this T obj) where T : Object
    {
        obj.hideFlags |= HideFlags.HideAndDontSave;
        return obj;
    }

    /// <summary>
    /// Stops <paramref name="obj"/> from being unloaded.
    /// 不卸载
    /// </summary>
    /// <param name="obj">The object to stop from being unloaded.</param>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>Passed <paramref name="obj"/>.</returns>
    public static T DontUnload<T>(this T obj) where T : Object
    {
        obj.hideFlags |= HideFlags.DontUnloadUnusedAsset;

        return obj;
    }

    /// <summary>
    /// Stops <paramref name="obj"/> from being destroyed on load.
    /// 加载后不销毁
    /// </summary>
    /// <param name="obj">The object to stop from being destroyed on load.</param>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>Passed <paramref name="obj"/>.</returns>
    public static T DontDestroyOnLoad<T>(this T obj) where T : Object
    {
        Object.DontDestroyOnLoad(obj);

        return obj;
    }

    /// <summary>
    /// Destroys the <paramref name="obj"/>.
    /// 摧毁
    /// </summary>
    /// <param name="obj">The object to destroy.</param>
    public static void Destroy(this Object obj)
    {
        Object.Destroy(obj);
    }

    /// <summary>
    /// Destroys the <paramref name="obj"/> immediately.
    /// 立刻摧毁
    /// </summary>
    /// <param name="obj">The object to destroy immediately.</param>
    public static void DestroyImmediate(this Object obj)
    {
        Object.DestroyImmediate(obj);
    }
}