using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NextShip.Api.Interfaces;

namespace NextShip.Manager;

[Harmony]
public class NextButtonsManager : INextButtonManager
{
    private readonly List<INextButton> allButtons = [];


    public void RegisterButton(INextButton button)
    {
        allButtons.Add(button);
    }

    public void UnRegisterButton(INextButton button)
    {
        allButtons.Remove(button);
    }

    public INextButton GetButton(Func<INextButton, bool> Finer)
    {
        return allButtons.FirstOrDefault(Finer);
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    [HarmonyPostfix]
    public static void OnHudManagerStart(HudManager __instance)
    {
    }


    [HarmonyPatch(typeof(HudManager), nameof(HudManager.OnGameStart))]
    [HarmonyPostfix]
    public static void OnHudManagerGameStart(HudManager __instance)
    {
    }
}