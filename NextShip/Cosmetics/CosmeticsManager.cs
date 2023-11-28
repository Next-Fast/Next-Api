using System.Collections.Generic;
using HarmonyLib;
using NextShip.Game.Info;
using NextShip.Manager;
using UnityEngine;

namespace NextShip.Cosmetics;

public class ShipCosmeticsManager : Manager<ShipCosmeticsManager>
{
    public List<ShipInfo> ShipInfos = new();
}

[HarmonyPatch]
internal static class ShipCosmeticsCache
{
}

[HarmonyPatch]
internal static class ShipCosmeticsSet
{
    [HarmonyPatch(typeof(PlayerMaterial), nameof(PlayerMaterial.SetColors), typeof(int), typeof(Renderer))]
    [HarmonyPrefix]
    public static bool PlayerMaterial_SetColors_Prefix1([HarmonyArgument(0)] int colorId,
        [HarmonyArgument(1)] Renderer rend)
    {
        return false;
    }

    [HarmonyPatch(typeof(PlayerMaterial), nameof(PlayerMaterial.SetColors), typeof(Color), typeof(Renderer))]
    [HarmonyPrefix]
    public static bool PlayerMaterial_SetColors_Prefix2([HarmonyArgument(0)] Color color,
        [HarmonyArgument(1)] Renderer rend)
    {
        return false;
    }
}