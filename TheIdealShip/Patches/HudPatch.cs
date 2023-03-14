using System;
using System.Linq;
using UnityEngine;
using HarmonyLib;
using TheIdealShip.Utilities;
using TheIdealShip.Roles;
using static TheIdealShip.Languages.Language;
using AmongUs.Data;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(TaskPanelBehaviour),nameof(TaskPanelBehaviour.SetTaskText))]
    class TaskPanelBehaviourPatch
    {
        public static void Postfix(TaskPanelBehaviour __instance)
        {
            var LPlayer = CachedPlayer.LocalPlayer.PlayerControl;
            var roleInfo = RoleHelpers.GetRoleInfo(LPlayer, false);
            var modifierInfo = RoleHelpers.GetRoleInfo(LPlayer, true);
            string roleText = "";
            string modifierText = "";
            if (roleInfo != null) roleText = Helpers.cs(roleInfo.color, GetString("Roles") + ":" + RoleHelpers.GetRolesString(LPlayer, false) + "\n"+ roleInfo.TaskText +"\n");
            if (modifierInfo != null) modifierText = Helpers.cs(modifierInfo.color, GetString("Modifiers") + ":" + RoleHelpers.GetRolesString(LPlayer, false, true) + "\n" + (modifierInfo.roleId == RoleId.Lover && RoleHelpers.getLover2() != null ? string.Format(modifierInfo.TaskText, RoleHelpers.getLover2().name) : modifierInfo.TaskText) + "\n");
            __instance.taskText.text = roleText + modifierText + "\n" + __instance.taskText.text;
//            __instance.taskText.text.Select(x => roleText + modifierText + ((roleInfo == null)&&(modifierInfo == null) ? "" : "\n") + x);
        }
    }
/*     [HarmonyPatch]
    public static class LoveChatPatch
    {
        public static ChatController LoverChat;

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update)), HarmonyPostfix]
        public static void LoverChat_Postfix(HudManager __instance)
        {
            if (!CachedPlayer.LocalPlayer.PlayerControl.IsLover()) return;
            if (LoverChat == null)
            {
                LoverChat = GameObject.Instantiate(__instance.Chat);
                LoverChat.name = "LoveChat";
                LoverChat.transform.SetParent(__instance.gameObject.transform);
                LoverChat.SetVisible(true);
                LoverChat.transform.position = __instance.Chat.transform.position;
            }
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
        public static bool LoverAddChat_Prefix(ChatController __instance,[HarmonyArgument(0)]PlayerControl sourcePlayer, [HarmonyArgument(1)]string chatText)
        {
            if (__instance.name != "LoveChat") return true;
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
            return false;
        }
    } */
}