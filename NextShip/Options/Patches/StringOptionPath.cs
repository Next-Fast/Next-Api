using System.Linq;
using HarmonyLib;

namespace NextShip.Options.Patches;

public class StringOptionPath
{
    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    public class StringOptionEnablePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            var option = OptionManager.AllOption.FirstOrDefault(option => option.OptionBehaviour == __instance);
            if (option == null) return true;

            __instance.OnValueChanged = null;
            __instance.TitleText.text = option.GetTitleString();
            __instance.Value = __instance.oldValue = option.GetInt();
            __instance.ValueText.text = option.GetValueString();

            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
    public class StringOptionIncreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            var option = OptionManager.AllOption.FirstOrDefault(option => option.OptionBehaviour == __instance);
            if (option == null) return true;

            option.Increase();
            return false;
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
    public class StringOptionDecreasePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            var option = OptionManager.AllOption.FirstOrDefault(option => option.OptionBehaviour == __instance);
            if (option == null) return true;

            option.Decrease();
            return false;
        }
    }
}