using System;
using HarmonyLib;

namespace NextShip.Chat;

[HarmonyPatch]
public static class Commands
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    [HarmonyPrefix]
    public static bool SendChatPatch_Prefix(ChatController __instance)
    {
        __instance.timeSinceLastMessage = 3f;
        var text = __instance.freeChatField.Text;
        var args = text.Split(' ');
        var canceled = false;
        var cancelVal = "";
        switch (args[0])
        {
            case "e":
                break;
        }

        if (Command.TryGetCommandEvent(text, out var action))
        {
            canceled = true;
            action();
        }


        if (!CheckChat(text, out var checkAction)) checkAction.Invoke(__instance);

        if (!canceled) return true;
        __instance.freeChatField.Clear();
        __instance.freeChatField.textArea.SetText(cancelVal);

        return false;
    }

    private static bool CheckChat(string text, out Action<ChatController> action)
    {
        action = null;
        return true;
    }
}