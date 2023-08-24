using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace NextShip.Utils;

public static class ObjetUtils
{
    public static List<Object> AllObjects = new ();
    public static List<Object> AllCacheObject = new ();

    public static  T Find<T>(string name) where T : Il2CppObjectBase
    {
        foreach (var obj in Resources.FindObjectsOfTypeAll(Il2CppType.Of<T>()))
        {
            if (obj.name == name)
            {
                Info($"ObjectUtils.Find return isnull:{false} Find<{typeof(T).Name}> Get:{name}");
                return obj.Cast<T>();
            }
        }
        Info($"ObjectUtils.Find return isnull:{true} Find<{typeof(T).Name}> Get:{name}");
        return null;
    }
    
    /*public  static void Init_Find<T>()
    {
        var all = Resources.FindObjectsOfTypeAll(Il2CppType.Of<T>());
        foreach (var Obj in all)
        {
            AllCacheObject.Add(Obj);
            Info($"Init_Find<{typeof(T).Name}> add:{Obj.name}");
        }
    }*/
    
    public static void Do(Object[] objects)
    {
        objects.Do(n =>
        {
            AllObjects.Add(n);
            Object.DontDestroyOnLoad(n);
            Info($"DontDestroyOnLoad : {n.name}");
        });
    }
}