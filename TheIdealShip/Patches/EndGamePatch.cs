using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TheIdealShip.Roles;
using TheIdealShip.Utilities;
using UnityEngine;
using static TheIdealShip.Languages.Language;

namespace TheIdealShip.Patches
{
    enum CustomGameOverReason
    {
        forcedEnd = 10,
        JesterWin
    }

    enum WinCondition
    {
        Default,
        JesterWin
    }

    static class AdditionalTempData
    {
        // Should be implemented using a proper GameOverReason in the future
        public static WinCondition winCondition = WinCondition.Default;
        public static List<WinCondition> additionalWinConditions = new List<WinCondition>();
        public static List<PlayerRoleInfo> playerRoles = new List<PlayerRoleInfo>();
        public static float timer = 0;

        public static void clear()
        {
            playerRoles.Clear();
            additionalWinConditions.Clear();
            winCondition = WinCondition.Default;
            timer = 0;
        }

        internal class PlayerRoleInfo
        {
            public string PlayerName { get; set; }
            public List<RoleInfo> Roles { get; set; }
            public int TasksCompleted { get; set; }
            public int TasksTotal { get; set; }
            public bool IsGuesser { get; set; }
            public int? Kills { get; set; }
        }
    }

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
    public  class OnGameEndPatch
    {
        public static void Postfix(AmongUsClient __instance, [HarmonyArgument(0)] ref EndGameResult endGameResult)
        {
            /* AdditionalTempData.clear();

            foreach(var player in CachedPlayer.AllPlayers)
            {
                var roles = RoleInfo.getRoleInfoForPlayer(player);
               /*  int? killCount = GameHistory.deadPlayers.FindAll(x => x.killerIfExisting != null && x.killerIfExisting.PlayerId == playerControl.PlayerId).Count;
                if (killCount == 0 && !(new List<RoleInfo>() { RoleInfo.sheriff, RoleInfo.jackal, RoleInfo.sidekick, RoleInfo.thief }.Contains(RoleInfo.getRoleInfoForPlayer(playerControl, false).FirstOrDefault()) || playerControl.Data.Role.IsImpostor))
                {
                    killCount = null;
                }
                AdditionalTempData.playerRoles.Add(new AdditionalTempData.PlayerRoleInfo() { PlayerName = playerControl.Data.PlayerName, Roles = roles, TasksTotal = tasksTotal, TasksCompleted = tasksCompleted, IsGuesser = isGuesser, Kills = killCount });
            } */

            List<PlayerControl> notWinners = new List<PlayerControl>();
            if (Jester.jester != null) notWinners.Add(Jester.jester);
            List<WinningPlayerData> winnersToRemove = new List<WinningPlayerData>();
            foreach (WinningPlayerData winner in TempData.winners.GetFastEnumerator())
            {
                if (notWinners.Any(x => x.Data.PlayerName == winner.PlayerName)) winnersToRemove.Add(winner);
            }

            foreach (var winner in winnersToRemove) TempData.winners.Remove(winner);
            var jesterWin = Jester.jester != null && endGameResult.GameOverReason == (GameOverReason)CustomGameOverReason.JesterWin;
            if (jesterWin)
            {
                TempData.winners = new Il2CppSystem.Collections.Generic.List<WinningPlayerData>();
                WinningPlayerData wpd = new WinningPlayerData(Jester.jester.Data);
                TempData.winners.Add(wpd);
                AdditionalTempData.winCondition = WinCondition.JesterWin;
            }
        }
    }

    [HarmonyPatch(typeof(EndGameManager), nameof(EndGameManager.SetEverythingUp))]
    public class EndGameManagerSetUpPatch
    {
        public static void Postfix(EndGameManager __instance)
        {
            GameObject bonusText = UnityEngine.Object.Instantiate(__instance.WinText.gameObject);
            bonusText.transform.position = new Vector3(__instance.WinText.transform.position.x, __instance.WinText.transform.position.y - 0.5f, __instance.WinText.transform.position.z);
            bonusText.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
            TMPro.TMP_Text textRenderer = bonusText.GetComponent<TMPro.TMP_Text>();
            textRenderer.text = "";

            if (TempData.EndReason == (GameOverReason)CustomGameOverReason.JesterWin)
            {
                __instance.WinText.text = GetString("JesterWinText");
                __instance.WinText.color = Jester.color;
                textRenderer.text = GetString("JesterWinSubText");
                textRenderer.color = Jester.color;
            }
            else if (TempData.EndReason == (GameOverReason)CustomGameOverReason.forcedEnd)
            {
                __instance.WinText.text = GetString("forcedEndWinText");
                __instance.WinText.color = Palette.AcceptedGreen;
            }
        }
    }
}