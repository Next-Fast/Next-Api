using HarmonyLib;

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
            }

            if (canceled)
            {
                __instance.freeChatField.Clear();
                __instance.freeChatField.textArea.SetText(cancelVal);
            }

            return !canceled;
        }
    }
}