using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Reflection;
using HarmonyLib;
using TheIdealShip.Modules;
using TheIdealShip.Utilities;
using UnityEngine;
using TheIdealShip.Roles;
using System.Collections.Generic;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(HudManager),nameof(HudManager.Update))]
    class HudManagerUpdatePatch
    {
        /*
        private static Dictionary<byte, (string name, Color color)> TagColorDict = new();
        static void resetNameTagsAndColors()
        {
            var localPlayer = CachedPlayer.LocalPlayer.PlayerControl;
            var myData = CachedPlayer.LocalPlayer.Data;
            var amImpostor = myData.Role.IsImpostor;

            var dict = TagColorDict;
            dict.Clear();

            foreach (var data in GameData.Instance.AllPlayers.GetFastEnumerator())
            {
                var player = data.Object;
                string text = data.PlayerName;
                Color color;
                if (player)
                {
                    var playerName = text;
                    var nameText = player.cosmetics.nameText;

                    nameText.text = playerName;
                    nameText.color = color = amImpostor && data.Role.IsImpostor ? Palette.ImpostorRed : Color.white;
                }
                else
                {
                    color = Color.white;
                }


                dict.Add(data.PlayerId, (text, color));
            }

            if (MeetingHud.Instance != null)
            {
                foreach (PlayerVoteArea playerVoteArea in MeetingHud.Instance.playerStates)
                {
                    var data = dict[playerVoteArea.TargetPlayerId];
                    var text = playerVoteArea.NameText;
                    text.text = data.name;
                    text.color = data.color;
                }
            }
        }
        */
        static void setPlayerNameColor(PlayerControl p,Color color)
        {
            p.cosmetics.nameText.color = color;
            if (MeetingHud.Instance != null)
            {
                foreach (PlayerVoteArea player in MeetingHud.Instance.playerStates)
                {
                    if (player.NameText != null && p.PlayerId == player.TargetPlayerId)
                    {
                        player.NameText.color = color;
                    }
                }
            }
        }
        static void SetNameColors()
        {
            var localPlayer = CachedPlayer.LocalPlayer.PlayerControl;
            var localRoleInfo = RoleInfo.getRoleInfoForPlayer(localPlayer, false).FirstOrDefault();
            setPlayerNameColor(localPlayer, localRoleInfo.color);
         //   if (localPlayer == Sheriff.sheriff)
         //   setPlayerNameColor(localPlayer, RoleInfo.sheriff.color);
        }
        static void updateVentButton(HudManager __instance)
        {
            if(MeetingHud.Instance) __instance.ImpostorVentButton.Hide();
            else if (!__instance.ImpostorVentButton.isActiveAndEnabled) __instance.ImpostorVentButton.Show();
        }

        static void updateUseButton(HudManager __instance)
        {
            if (MeetingHud.Instance) __instance.UseButton.Hide();
        }

        static void updateSabotageButton(HudManager __instance)
        {
            if (MeetingHud.Instance) __instance.SabotageButton.Hide();
        }

        static void updateMapButton(HudManager __instance)
        {
            if (__instance == null || __instance.MapButton.HeldButtonSprite == null) return;
            __instance.MapButton.HeldButtonSprite.color = Color.white;
        }

        static void Postfix(HudManager __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;
            CustomButton.HudUpdate();
            SetNameColors();
            updateVentButton(__instance);
            // Meeting hide buttons if needed (used for the map usage, because closing the map would show buttons)
            updateSabotageButton(__instance);
            updateUseButton(__instance);
            updateMapButton(__instance);
        }

    }
}