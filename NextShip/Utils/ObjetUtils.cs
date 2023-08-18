using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace NextShip.Utils;

public class ObjetUtils
{
    public static List<Object> AllObjects = new List<Object>();
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