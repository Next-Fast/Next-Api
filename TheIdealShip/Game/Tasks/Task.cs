using System;
using HarmonyLib;
using TheIdealShip.Utilities;

namespace TheIdealShip.Game.Tasks
{
    [HarmonyPatch]
    public static class TasksHandler
    {

        public static Tuple<int, int> taskInfo(GameData.PlayerInfo playerInfo)
        {
            int TotalTasks = 0;
            int CompletedTasks = 0;
            if (!playerInfo.Disconnected &&
                playerInfo.Tasks != null &&
                playerInfo.Object &&
                playerInfo.Role &&
                playerInfo.Role.TasksCountTowardProgress
                )
            {
                foreach (var playerInfoTask in playerInfo.Tasks.GetFastEnumerator())
                {
                    if (playerInfoTask.Complete) CompletedTasks++;
                    TotalTasks++;
                }
            }
            return Tuple.Create(CompletedTasks, TotalTasks);
        }

        [HarmonyPatch(typeof(GameData), nameof(GameData.RecomputeTaskCounts))]
        private static class GameDataRecomputeTaskCountsPatch
        {
            private static bool Prefix(GameData __instance)
            {
                var totalTasks = 0;
                var completedTasks = 0;

                foreach (var playerInfo in GameData.Instance.AllPlayers.GetFastEnumerator())
                {
                    var (playerCompleted, playerTotal) = taskInfo(playerInfo);
                    totalTasks += playerTotal;
                    completedTasks += playerCompleted;
                }

                __instance.TotalTasks = totalTasks;
                __instance.CompletedTasks = completedTasks;
                return false;
            }
        }

    }
}