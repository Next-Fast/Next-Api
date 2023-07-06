using HarmonyLib;
using Hazel;
using TheIdealShip.Roles;
using TheIdealShip.RPC;
using TheIdealShip.Utilities;
using UnityEngine;

namespace TheIdealShip;

[HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
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
            Warn("创建技能失败", filename: "Button");
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
                    targetId = Sheriff.currentTarget.PlayerId;
                else
                    targetId = CachedPlayer.LocalPlayer.PlayerId;

                var killWriter = AmongUsClient.Instance.StartRpcImmediately(
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
            () => { sheriffKillButton.Timer = sheriffKillButton.MaxTimer; },
            () => { return !PlayerControl.LocalPlayer.Data.IsDead; },
            () =>
            {
                return Sheriff.currentTarget != null && Sheriff.shootNumber > 0 && PlayerControl.LocalPlayer.CanMove;
            },
            __instance.KillButton.graphic.sprite,
            new Vector3(0f, 1f, 0),
            __instance,
            KeyCode.Q,
            RoleId.Sheriff
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
                CamouflagerButton.Timer = CamouflagerButton.MaxTimer;
                CamouflagerButton.isEffectActive = false;
                CamouflagerButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            () => { return !PlayerControl.LocalPlayer.Data.IsDead; },
            () => { return PlayerControl.LocalPlayer.CanMove; },
            Camouflager.getButtonSprite(),
            new Vector3(0f, 2f, 0),
            __instance,
            KeyCode.F,
            RoleId.Camouflager,
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
                IllusoryButton.Timer = IllusoryButton.MaxTimer;
                IllusoryButton.isEffectActive = false;
                IllusoryButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
            },
            () => { return !PlayerControl.LocalPlayer.Data.IsDead; },
            () => { return PlayerControl.LocalPlayer.CanMove; },
            Illusory.getButtonSprite(),
            new Vector3(0f, 2f, 0),
            __instance,
            KeyCode.F,
            RoleId.Illusory,
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