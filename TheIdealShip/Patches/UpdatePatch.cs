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
            var localRoleInfo = RoleHelpers.GetRoleInfo(localPlayer, false);
            setPlayerNameColor(localPlayer, localRoleInfo.color);
        }
        static void updateVentButton(HudManager __instance)
        {
            if(MeetingHud.Instance || CachedPlayer.LocalPlayer.Data.IsDead) __instance.ImpostorVentButton.Hide();
            else if (CachedPlayer.LocalPlayer.PlayerControl.Data.Role.IsImpostor &&!__instance.ImpostorVentButton.isActiveAndEnabled) __instance.ImpostorVentButton.Show();
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
            updateSabotageButton(__instance);
            updateUseButton(__instance);
            updateMapButton(__instance);
        }

    }
}