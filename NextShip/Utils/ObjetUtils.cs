using System.Collections.Generic;
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
        bool find = false;
        Object GetObject = null;
        
        foreach (var Obj in Resources.FindObjectsOfTypeAll(Il2CppType.Of<T>()))
        {
            if (Obj.name == name)
            {
                find = true;
                GetObject = Obj;
            }
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
}