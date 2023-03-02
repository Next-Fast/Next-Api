using HarmonyLib;
using TheIdealShip.Utilities;
using Il2CppInterop.Runtime;
using BepInEx.Unity.IL2CPP.Utils.Collections;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(GameStartManager),nameof(GameStartManager.Update))]
    class GameStartManagerUpdatePatch
    {
        public static void Prefix(GameStartManager __instance)
        {
            float time = __instance.countDownTimer;
            if (CustomOptionHolder.noGameEnd.getBool())
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
    class GameStartManagerBeginGamePatch
    {
        public static void Prefix()
        {
            if (CustomOptionHolder.noGameEnd.getBool() && CustomOptionHolder.dummynumber.getSelection() != 0)
            {
                int num = CustomOptionHolder.dummynumber.getSelection();
                for (int n = 1; n < num; n++)
                {
                    var playerControl = UnityEngine.Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);
                    var i = playerControl.PlayerId = (byte)GameData.Instance.GetAvailableId();

                    GameData.Instance.AddPlayer(playerControl);
                    AmongUsClient.Instance.Spawn(playerControl, -2, InnerNet.SpawnFlags.None);

                    playerControl.transform.position = PlayerControl.LocalPlayer.transform.position;
                    playerControl.GetComponent<DummyBehaviour>().enabled = true;
                    playerControl.isDummy = true;
                    playerControl.SetName("假人" + n.ToString());
                    playerControl.SetColor(i);

                    GameData.Instance.RpcSetTasks(playerControl.PlayerId, new byte[0]);
                }
            }
        }
    }
}