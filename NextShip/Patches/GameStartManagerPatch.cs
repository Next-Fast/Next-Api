using HarmonyLib;
using UnityEngine;

namespace NextShip.Patches;

[HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
internal class GameStartManagerUpdatePatch
{
    public static void Prefix(GameStartManager __instance)
    {
        var time = __instance.countDownTimer;
        if (noGameEnd.getBool())
        {
            __instance.MinPlayers = 1;
            __instance.countDownTimer = 0;
        }
        else
        {
            __instance.MinPlayers = 4;
            __instance.countDownTimer = time;
        }
    }
}

[HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
internal class GameStartManagerBeginGamePatch
{
    public static void Prefix()
    {
        if (noGameEnd.getBool() && dummynumber.getSelection() != 0)
        {
            var num = dummynumber.getSelection();
            for (var n = 1; n < num; n++)
            {
                var playerControl = Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);
                var i = playerControl.PlayerId = (byte)GameData.Instance.GetAvailableId();

                GameData.Instance.AddPlayer(playerControl);
                AmongUsClient.Instance.Spawn(playerControl);

                playerControl.transform.position = PlayerControl.LocalPlayer.transform.position;
                playerControl.GetComponent<DummyBehaviour>().enabled = true;
                playerControl.isDummy = true;
                playerControl.SetName("假人" + n);
                playerControl.SetColor(i);

                GameData.Instance.RpcSetTasks(playerControl.PlayerId, new byte[0]);
            }
        }
    }
}