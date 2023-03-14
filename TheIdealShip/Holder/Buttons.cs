using HarmonyLib;
using Hazel;
using InnerNet;
// using Reactor.Networking.Rpc;
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
        public static CustomButton IllusoryButton;

        public static void setCustomButtonCooldowns()
        {
            sheriffKillButton.MaxTimer = Sheriff.cooldown;
            CamouflagerButton.MaxTimer = Camouflager.cooldown;
            IllusoryButton.MaxTimer = Illusory.cooldown;
        }

        public static void Postfix(HudManager __instance)
        {
            try
            {
                CreateButton(__instance);
            }
            catch
            {
                Warn("创建技能失败", filename:"Button");
            }
        }

        public static void CreateButton(HudManager __instance)
        {
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
                    return Sheriff.sheriff == PlayerControl.LocalPlayer;
                    //                  return Sheriff.sheriff.RoleIsH() && Sheriff.sheriff.Is(LocalPlayer) && LocalPlayer.IsSurvival();
                },
                () =>
                {
                    return Sheriff.currentTarget && Sheriff.shootNumber > 0;
                },
                () =>
                {
                    sheriffKillButton.Timer = sheriffKillButton.MaxTimer;
                },
                __instance.KillButton.graphic.sprite,
                new Vector3(0f, 1f, 0),
                __instance,
                KeyCode.Q
            );

            // 隐蔽（伪装）技能
            CamouflagerButton = new CustomButton
            (
                () =>
                {
                    RPCHelpers.Create((byte)CustomRPC.Camouflager);
                    RPCProcedure.Camouflager();
                },
                () =>
                {
                    return Camouflager.camouflager == PlayerControl.LocalPlayer;
                },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    CamouflagerButton.Timer = CamouflagerButton.MaxTimer;
                    CamouflagerButton.isEffectActive = false;
                    CamouflagerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Camouflager.getButtonSprite(),
                new Vector3(0f, 1f, 0),
                __instance,
                KeyCode.F,
                true,
                Camouflager.duration,
                () =>
                {
                    CamouflagerButton.Timer = CamouflagerButton.MaxTimer;
                    RPCHelpers.Create((byte)CustomRPC.RestorePlayerLook);
                    RPCProcedure.RestorePlayerLook();
                }
            );

            // 虚影技能
            IllusoryButton = new CustomButton
            (
                () =>
                {
                    RPCHelpers.Create((byte)CustomRPC.Illusory);
                    RPCProcedure.Illusory();
                },
                () =>
                {
                    return Illusory.illusory == PlayerControl.LocalPlayer;
                },
                () =>
                {
                    return PlayerControl.LocalPlayer.CanMove;
                },
                () =>
                {
                    IllusoryButton.Timer = IllusoryButton.MaxTimer;
                    IllusoryButton.isEffectActive = false;
                    IllusoryButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
                },
                Illusory.getButtonSprite(),
                new Vector3(0f, 1f, 0),
                __instance,
                KeyCode.F,
                true,
                Illusory.duration,
                () =>
                {
                    RPCHelpers.Create((byte)CustomRPC.RestorePlayerLook);
                    RPCProcedure.RestorePlayerLook();
                }
            );

            setCustomButtonCooldowns();
        }
    }
}