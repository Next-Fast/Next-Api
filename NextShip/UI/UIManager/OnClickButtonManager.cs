using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NextShip.UI.Components;
using UnityEngine;

namespace NextShip.UI.UIManager;

public static class OnClickButtonManager
{
    public static readonly List<OnClickButton> AllOnClickButtons = new ();
    private static List<BoxCollider2D> BoxCollider2DCache = new ();
    private static List<Box> _boxs = new ();
        
    [HarmonyPatch(typeof(PassiveButtonManager), nameof(PassiveButtonManager.Update)), HarmonyPostfix]
    public static void ManagerUpdate(PassiveButtonManager __instance)
    {
        CacheBox();
        Update(__instance);
    }

    private static void CacheBox()
    {
        int boxCount = 0;
        int boxCacheCount = BoxCollider2DCache.Count;
        foreach (var Button in AllOnClickButtons)
        {
            boxCount += Button.BoxCollider2Ds.Length;
        }

        if (boxCount == boxCacheCount) return;

        BoxCollider2DCache = new List<BoxCollider2D>();
        foreach (var Button in AllOnClickButtons)
        {
            BoxCollider2DCache.AddRange(Button.BoxCollider2Ds);
            Button.BoxCollider2Ds.Do(n => _boxs.Add(new Box(n, Button)));
        }
    }

    private static void Update(PassiveButtonManager __instance)
    {
        foreach (var box in BoxCollider2DCache)
        {
            if (!box.isActiveAndEnabled || !box) return;
            switch (__instance.controller.CheckDrag(box))
            {
                case DragState.TouchStart:
                    _boxs.FirstOrDefault(n => n.BoxCollider2D == box)!.Button.OnClick.Invoke();
                    break;
            }
        }
    }

    internal static void Register(this OnClickButton button) =>
        AllOnClickButtons.Add(button);

    private record Box(BoxCollider2D BoxCollider2D, OnClickButton Button);
}