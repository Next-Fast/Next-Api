using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Hazel;
using TheIdealShip.Roles;
using TheIdealShip.Utilities;
using UnityEngine;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(IntroCutscene),nameof(IntroCutscene.ShowRole))]
    class ShowRolePatch
    {
        public static void SetRoleTexts(IntroCutscene __instance)
        {
            var LocalP = CachedPlayer.LocalPlayer.PlayerControl;
//  获取本地玩家角色信息
            RoleInfo roleInfo = RoleHelpers.GetRoleInfo(LocalP, false);
            RoleInfo modifierInfo = RoleHelpers.GetRoleInfo(LocalP, true);
            if (roleInfo != null)
            {
                __instance.YouAreText.color = roleInfo.color;
                __instance.RoleText.text = roleInfo.name;
                __instance.RoleText.color = roleInfo.color;
                __instance.RoleBlurbText.text = roleInfo.IntroD;
                __instance.RoleBlurbText.color = roleInfo.color;
            }
            if (modifierInfo != null)
            {
                __instance.RoleText.text += Helpers.cs(modifierInfo.color, $" {modifierInfo.name}");
                __instance.RoleBlurbText.text += Helpers.cs(modifierInfo.color, $"\n{modifierInfo.IntroD}");
            }
        }
        public static bool Prefix(IntroCutscene __instance)
        {
            if (!CustomOptionHolder.activateRoles.getBool()) return true;
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>{ SetRoleTexts(__instance);})));
            return true;
        }
    }

    [HarmonyPatch(typeof(IntroCutscene),nameof(IntroCutscene.ShowTeam))]
    class ShowTeamPatch
    {
        public static void setRoleTeamText(IntroCutscene __instance)
        {
            var LocalP = CachedPlayer.LocalPlayer.PlayerControl;
            var teamText = RoleHelpers.GetRoleTeam(LocalP);
            __instance.TeamTitle.text = teamText;
        }
        public static bool Prefix(IntroCutscene __instance)
        {
            if (!CustomOptionHolder.activateRoles.getBool()) return true;
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) => { setRoleTeamText(__instance); })));
            return true;
        }
    }
}