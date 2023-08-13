using HarmonyLib;

namespace NextShip.Options.Patches;

[HarmonyPatch]
public class OptionMenuPatch
{
    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start)), HarmonyPrefix]
    public static bool GameOptionMenu_Start_Prefix() => false;
    
    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start)), HarmonyPrefix]
    public static bool GameSettingMenu_Start_Prefix() => false;
    
    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update)), HarmonyPrefix]
    public static bool GameOptionMenu_Update_Prefix() => false;
    
    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Update)), HarmonyPrefix]
    public static bool GameSettingMenu_Update_Prefix() => false;
}