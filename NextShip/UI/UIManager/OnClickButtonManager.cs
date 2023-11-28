using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NextShip.UI.Components;
using UnityEngine;

namespace NextShip.UI.UIManager;

[HarmonyPatch]
public class OnClickButtonManager
{
    public static OnClickButtonManager Instance;
    private readonly List<Box> _boxs = new();

    public readonly List<OnClickButton> AllOnClickButtons = new();
    private List<BoxCollider2D> BoxCollider2DCache = new();

    [HarmonyPatch(typeof(PassiveButtonManager), nameof(PassiveButtonManager.Update))]
    [HarmonyPostfix]
    public static void ManagerUpdate(PassiveButtonManager __instance)
    {
        Get().CacheBox();
        Get().Update(__instance);
    }

    private void CacheBox()
    {
        var boxCacheCount = BoxCollider2DCache.Count;
        var boxCount = AllOnClickButtons.Sum(Button => Button.BoxCollider2Ds.Length);

        if (boxCount == boxCacheCount) return;

        BoxCollider2DCache = new List<BoxCollider2D>();
        foreach (var Button in AllOnClickButtons)
        {
            BoxCollider2DCache.AddRange(Button.BoxCollider2Ds);
            Button.BoxCollider2Ds.Do(n => _boxs.Add(new Box(n, Button)));
        }
    }

    private void Update(PassiveButtonManager __instance)
    {
        foreach (var box in BoxCollider2DCache)
        {
            if (!box.isActiveAndEnabled || !box) return;
            switch (__instance.controller.CheckDrag(box))
            {
                case DragState.TouchStart:
                    Start(box);
                    break;
                case DragState.NoTouch:
                    Start(box);
                    break;
                case DragState.Holding:
                    Start(box);
                    break;
                case DragState.Dragging:
                    Start(box);
                    break;
                case DragState.Released:
                    Start(box);
                    break;
                default:
                    Info("defaut");
                    break;
            }
        }
    }

    private void Start(BoxCollider2D box)
    {
        _boxs.FirstOrDefault(n => n.BoxCollider2D == box)!.Button.OnClick.Invoke();
    }

    internal void Register(OnClickButton button)
    {
        AllOnClickButtons.Add(button);
    }

    public static OnClickButtonManager Get()
    {
        return Instance ??= new OnClickButtonManager();
    }

    private record Box(BoxCollider2D BoxCollider2D, OnClickButton Button);
}