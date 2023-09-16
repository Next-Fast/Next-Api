using System.Linq;
using HarmonyLib;
using NextShip.Chat.Patches;

namespace NextShip.Chat;

[HarmonyPatch]
public static class Commands
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    private static class SendChatPatch
    {
        public static bool Prefix(ChatController __instance)
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

            if (Command.AllCommand.Any(command => command.GetText() == text))
            {
                canceled = true;
            }

            if (!canceled) return true;
            __instance.freeChatField.Clear();
            __instance.freeChatField.textArea.SetText(cancelVal);

            return false;
        }
    }
}