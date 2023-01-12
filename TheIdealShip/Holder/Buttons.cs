using HarmonyLib;
using Hazel;
using TheIdealShip.Modules;
using TheIdealShip.Roles;
using TheIdealShip.Utilities;
using UnityEngine;

namespace TheIdealShip
{
    [HarmonyPatch(typeof(HudManager),nameof(HudManager.Start))]
    public static class HudManagerStartPatch
    {
        public static CustomButton sheriffKillButton;

        public static void setCustomButtonCooldowns()
        {
            sheriffKillButton.MaxTimer = Sheriff.cooldown;
        }

        public static void Postfix(HudManager __instance)
        {
         // 警长击杀 (Sheriff kill)
            sheriffKillButton = new CustomButton
            (
                () => {
                    byte targetId = 0;
                    if (Sheriff.currentTarget.Data.Role.IsImpostor)
                    {
                        targetId = Sheriff.currentTarget.PlayerId;
                    }
                    else
                    {
                        targetId = CachedPlayer.LocalPlayer.PlayerId;
                    }

                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(
                        CachedPlayer.LocalPlayer.PlayerControl.NetId,
                        (byte)CustomRPC.SheriffKill, SendOption.Reliable, -1
                    );
                    killWriter.Write(Sheriff.sheriff.Data.PlayerId);
                    killWriter.Write(targetId);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.SheriffKill(targetId);
                    sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                    Sheriff.currentTarget = null;
                    Sheriff.shootNumber--;
                },
                () => {
                    return Sheriff.sheriff != null && Sheriff.sheriff == CachedPlayer.LocalPlayer.PlayerControl && !CachedPlayer.LocalPlayer.Data.IsDead;
                },
                () => {
                    return Sheriff.currentTarget && CachedPlayer.LocalPlayer.PlayerControl.CanMove && Sheriff.shootNumber > 0;
                },
                () => {
                    sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                },
                __instance.KillButton.graphic.sprite,
                new Vector3(0f,1f,0),
                __instance,
                KeyCode.Q
            );

            setCustomButtonCooldowns();
        }
    }
}