using UnityEngine;
using HarmonyLib;
using TheIdealShip.Utilities;
using TheIdealShip.RPC;
using AmongUs.Data;

namespace TheIdealShip.Patches
{
    [HarmonyPatch]
    public static class LoveChatPatch
    {
        public static ChatController LoverChat;

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update)), HarmonyPostfix]
        public static void LoverChat_Postfix(HudManager __instance)
        {
            if (CustomOptionHolder.noGameEnd.getBool())
            {
                if(!__instance.Chat.isActiveAndEnabled) __instance.Chat.SetVisible(true);
            }
            if (!CachedPlayer.LocalPlayer.PlayerControl.IsLover()) return;
            if (MeetingHud.Instance)
            {
                LoverChat.transform.position = __instance.Chat.transform.position + new Vector3(-0.5f, 0, 0);
            }
            else
            {
                LoverChat.transform.position = __instance.Chat.transform.position;
            }
        }
        [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat)), HarmonyPrefix]
        public static bool LoverSenndChat_Prefix(ChatController __instance)
        {
            if (__instance.name != "LoveChat") return true;
            string text = __instance.TextArea.text;
            RPCProcedure.LoverSendChat(PlayerControl.LocalPlayer, text, true);
            __instance.TextArea.Clear();
            return false;
        }

        [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat)), HarmonyPrefix]
        public static bool AddChat_Prefix(ChatController __instance,[HarmonyArgument(0)]PlayerControl sourcePlayer, [HarmonyArgument(1)]string chatText)
        {
            if (!CheckForModification(__instance, sourcePlayer, chatText)) return true;
            CreateAddChat(__instance, sourcePlayer, chatText);
            return false;
        }

        private static void CreateChat(HudManager __instance, ChatController chat, string chatName, bool Visible, Vector3 chatPos)
        {
            if (chat != null)
            {
                if(chat.transform.position != __instance.Chat.transform.position + chatPos) chat.transform.position = __instance.Chat.transform.position + chatPos;
                if(chat.gameObject.active != Visible) chat.SetVisible(Visible);
                return;
            }

            chat = GameObject.Instantiate(__instance.Chat);
            chat.name = chatName;
            chat.transform.SetParent(__instance.gameObject.transform);
            chat.SetVisible(Visible);
            chat.transform.position = __instance.Chat.transform.position + chatPos;
        }

        private static void CreateAddChat(ChatController __instance, PlayerControl sourcePlayer, string chatText)
        {
            GameData.PlayerInfo data = PlayerControl.LocalPlayer.Data;
            GameData.PlayerInfo data2 = sourcePlayer.Data;
            ChatBubble chatBubble = __instance.chatBubPool.Get<ChatBubble>();
            chatBubble.transform.SetParent(__instance.scroller.Inner);
            chatBubble.transform.localScale = Vector3.one;
            bool flag = sourcePlayer == PlayerControl.LocalPlayer;
            if (flag)
            {
                chatBubble.SetRight();
            }
            else
            {
                chatBubble.SetLeft();
            }
            bool didVote = MeetingHud.Instance && MeetingHud.Instance.DidVote(sourcePlayer.PlayerId);
            chatBubble.SetCosmetics(data2);
            __instance.SetChatBubbleName(chatBubble, data2, data2.IsDead, didVote, PlayerNameColor.Get(data2), null);
            if (DataManager.Settings.Multiplayer.CensorChat)
            {
                chatText = BlockedWords.CensorWords(chatText, false);
            }
            chatBubble.SetText(chatText);
            chatBubble.AlignChildren();
            __instance.AlignAllBubbles();
            if (!flag)
            {
                SoundManager.Instance.PlaySound(__instance.MessageSound, false, 1f, null).pitch = 0.5f + (float)sourcePlayer.PlayerId / 15f;
            }
        }

        private static bool CheckForModification(ChatController __instance, PlayerControl sourcePlayer, string chatText)
        {
            if (__instance.name == "LoverChat") return true;

            return false;
        }
    }
}