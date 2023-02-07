using HarmonyLib;
using Hazel;
using Reactor.Networking.Rpc;
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
        public static CustomButton CamouflagerButton;

        public static void setCustomButtonCooldowns()
        {
            sheriffKillButton.MaxTimer = Sheriff.cooldown;
            CamouflagerButton.MaxTimer = Camouflager.cooldown;
        }

        public static void Postfix(HudManager __instance)
        {
            var LocalPlayer = CachedPlayer.LocalPlayer.PlayerControl;
         // 警长击杀 (Sheriff kill)
            sheriffKillButton = new CustomButton
            (
                () =>
                {
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
                        (byte)CustomRPC.SheriffKill, SendOption.Reliable);
                    killWriter.Write(Sheriff.sheriff.Data.PlayerId);
                    killWriter.Write(targetId);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.SheriffKill(targetId);
                    sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                    Sheriff.currentTarget = null;
                    Sheriff.shootNumber--;
                },
                () =>
                {
                    return Sheriff.sheriff != null && Sheriff.sheriff == LocalPlayer && !LocalPlayer.Data.IsDead;
//                  return Sheriff.sheriff.RoleIsH() && Sheriff.sheriff.Is(LocalPlayer) && LocalPlayer.IsSurvival();
                },
                () =>
                {
                    return Sheriff.currentTarget && LocalPlayer.CanMove && Sheriff.shootNumber > 0;
                },
                () =>
                {
                    sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                },
                __instance.KillButton.graphic.sprite,
                new Vector3(0f,1f,0),
                __instance,
                KeyCode.Q
            );

            CamouflagerButton = new CustomButton
            (
                () => 
                {
                    RPCHelpers.Create((byte)CustomRPC.Camouflager,bools:new bool[]{false});
                    RPCProcedure.Camouflager(false);
                },
                () =>
                {
                    return Camouflager.camouflager != null && Camouflager.camouflager == LocalPlayer && !LocalPlayer.Data.IsDead;
                },
                () => 
                {
                    return LocalPlayer.CanMove;
                },
                () => 
                {
                    CamouflagerButton.Timer = CamouflagerButton.MaxTimer;
                    CamouflagerButton.isEffectActive = false;
                    CamouflagerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Camouflager.getButtonSprite(),
                new Vector3(0f,1f,0),
                __instance,
                KeyCode.Q,
                true,
                Camouflager.duration,
                () =>
                {
                    CamouflagerButton.Timer = CamouflagerButton.MaxTimer;
                    RPCHelpers.Create((byte)CustomRPC.Camouflager,bools:new bool[]{true});
                    RPCProcedure.Camouflager(true);
                }
            );

            setCustomButtonCooldowns();
        }
    }
}