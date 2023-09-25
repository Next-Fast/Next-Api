using System.Linq;
using HarmonyLib;
using Il2CppSystem;

namespace NextShip.Options.Patches;

[HarmonyPatch(typeof(StringOption))]
public class StringOptionPath
{
    [HarmonyPatch(nameof(StringOption.OnEnable)), HarmonyPrefix]
    public static bool StringOptionEnablePatch_Prefix(StringOption __instance)
    {
        if (OptionsConsolePatch.IsNextMenu) return false;
        var option = OptionManager.AllOption.FirstOrDefault(option => option.OptionBehaviour == __instance);
        if (option == null) return true;

        __instance.OnValueChanged = null;
        OnOptionUpdate(__instance, option);
        return false;
    }
    

    [HarmonyPatch(nameof(StringOption.Increase)), HarmonyPrefix]
    public static bool StringOptionIncreasePatch_Prefix(StringOption __instance)
    {
        if (OptionsConsolePatch.IsNextMenu) return false;
        var option = OptionManager.AllOption.FirstOrDefault(option => option.OptionBehaviour == __instance);
        if (option == null) return true;

        option.Increase();
        OnOptionUpdate(__instance, option);
        return false;
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    public static bool StringOptionDecreasePatch_Prefix(StringOption __instance)
    {
        if (OptionsConsolePatch.IsNextMenu) return false;
        var option = OptionManager.AllOption.FirstOrDefault(option => option.OptionBehaviour == __instance);
        if (option == null) return true;

        option.Decrease();
        OnOptionUpdate(__instance, option);
        return false;
    }

    private static void OnOptionUpdate(StringOption __instance, OptionBase option)
    {
        __instance.TitleText.text = option.GetTitleString();
        __instance.Value = __instance.oldValue = option.GetInt();
        __instance.ValueText.text = option.GetValueString();
    }
}