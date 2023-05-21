using HarmonyLib;
using TheIdealShip.Roles;
using TheIdealShip.Utilities;
using TMPro;
using UnityEngine;
using AmongUs.GameOptions;
using System.Linq;
using System.Collections.Generic;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class PlayerControlFixedUpdatePatch
    {
        static PlayerControl setTarget(bool onlyCrewmates = false, bool targetPlayersInVents = false,
            List<PlayerControl> untargetablePlayers = null, PlayerControl targetingPlayer = null)
        {
            var Go = GameOptionsManager.Instance.currentGameOptions;
            var NGO = GameOptionsManager.Instance.currentNormalGameOptions;
            PlayerControl result = null;
            float num = GameOptionsData.KillDistances[Mathf.Clamp(NGO.KillDistance, 0, 2)];
            if (!ShipStatus.Instance) return result;
            if (targetingPlayer == null) targetingPlayer = CachedPlayer.LocalPlayer.PlayerControl;
            if (targetingPlayer.Data.IsDead) return result;

            Vector2 truePosition = targetingPlayer.GetTruePosition();
            foreach (var playerInfo in GameData.Instance.AllPlayers.GetFastEnumerator())
            {
                if (!playerInfo.Disconnected && playerInfo.PlayerId != targetingPlayer.PlayerId && !playerInfo.IsDead &&
                    (!onlyCrewmates || !playerInfo.Role.IsImpostor))
                {
                    PlayerControl @object = playerInfo.Object;
                    if (untargetablePlayers != null && untargetablePlayers.Any(x => x == @object))
                    {
                        // if that player is not targetable: skip check
                        continue;
                    }

                    if (@object && (!@object.inVent || targetPlayersInVents))
                    {
                        Vector2 vector = @object.GetTruePosition() - truePosition;
                        float magnitude = vector.magnitude;
                        if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized,
                                magnitude, Constants.ShipAndObjectsMask))
                        {
                            result = @object;
                            num = magnitude;
                        }
                    }
                }
            }

            return result;
        }

        static void setPlayerOutline(PlayerControl target, Color color)
        {
            if (target == null || target.cosmetics?.currentBodySprite?.BodySprite == null) return;

            target.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 1f);
            target.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", color);
        }

        static void setBasePlayerOutlines()
        {
            foreach (PlayerControl target in PlayerControl.AllPlayerControls)
            {
                if (target == null || target.cosmetics?.currentBodySprite?.BodySprite.material == null)
                    continue;
                target.cosmetics?.currentBodySprite?.BodySprite.material.SetFloat("_Outline", 0f);
            }
        }

        static void sheriffSetTarget()
        {
            if (Sheriff.sheriff == null || Sheriff.sheriff != CachedPlayer.LocalPlayer.PlayerControl)
                return;
            Sheriff.currentTarget = setTarget();
            setPlayerOutline(Sheriff.currentTarget, Sheriff.color);
        }

        static void impostorSetTarget()
        {
            if (!CachedPlayer.LocalPlayer.Data.Role.IsImpostor || !CachedPlayer.LocalPlayer.PlayerControl.CanMove ||
                CachedPlayer.LocalPlayer.Data.IsDead)
            {
                FastDestroyableSingleton<HudManager>.Instance.KillButton.SetTarget(null);
                return;
            }

            PlayerControl target = null;
            target = setTarget(true, true);

            FastDestroyableSingleton<HudManager>.Instance.KillButton
                .SetTarget(target); // Includes setPlayerOutline(target, Palette.ImpstorRed);
        }

        public static void updatePlayerInfo()
        {
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if (p == CachedPlayer.LocalPlayer.PlayerControl || CachedPlayer.LocalPlayer.Data.IsDead || p.isDummy)
                {
                    Transform playerInfoTransform = p.cosmetics.nameText.transform.parent.FindChild("Info");
                    TextMeshPro playerInfo = playerInfoTransform != null
                        ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>()
                        : null;
                    if (playerInfo == null)
                    {
                        playerInfo = UnityEngine.Object.Instantiate(p.cosmetics.nameText,
                            p.cosmetics.nameText.transform.parent);
                        playerInfo.transform.localPosition += Vector3.up * 0.225f;
                        playerInfo.fontSize *= 0.75f;
                        playerInfo.gameObject.name = "Info";
                        playerInfo.color = playerInfo.color.SetAlpha(1f);
                    }

                    PlayerVoteArea playerVoteArea = MeetingHud.Instance?.playerStates?.FirstOrDefault(x => x.TargetPlayerId == p.PlayerId);
                    Transform meetingInfoTransform = playerVoteArea != null ? playerVoteArea.NameText.transform.parent.FindChild("Info") : null;
                    TMPro.TextMeshPro meetingInfo = meetingInfoTransform != null ? meetingInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                    if (meetingInfo == null && playerVoteArea != null)
                    {
                        meetingInfo = UnityEngine.Object.Instantiate(playerVoteArea.NameText, playerVoteArea.NameText.transform.parent);
                        meetingInfo.transform.localPosition += Vector3.down * 0.20f;
                        meetingInfo.fontSize *= 0.75f;
                        meetingInfo.gameObject.name = "Info";
                    }

                    string roleNames = RoleHelpers.GetRolesString(p, true);
                    string modifierName = RoleHelpers.GetRolesString(p, true, true);

                    string playerInfoText = "";
                    string meetingInfoText = "";
                    if (p == CachedPlayer.LocalPlayer.PlayerControl || p.isDummy)
                    {
                        playerInfoText = $"{roleNames}";
                        meetingInfoText = $"{roleNames}\n{modifierName}".Trim();
                    }

                    playerInfo.text = playerInfoText;
                    playerInfo.gameObject.SetActive(p.Visible);
                    if (meetingInfo != null)
                        meetingInfo.text = MeetingHud.Instance.state == MeetingHud.VoteStates.Results ? "" : meetingInfoText;
                }
            }
        }

        public static void speedUpdate()
        {
            var p = CachedPlayer.LocalPlayer.PlayerControl;
            if (p == Flash.flash && Flash.flash != null)
            {
                p.MyPhysics.Speed = Flash.speed;
            }
        }

        public static void GhostSpeedUpdate()
        {
            if (CustomOptionHolder.PlayerOption.getBool())
            {
                for (var i = 0; i < PlayerControl.AllPlayerControls.Count; i++)
                {
                    PlayerControl.AllPlayerControls[i].MyPhysics.GhostSpeed =
                        CustomOptionHolder.PlayerGhostSpeed.getFloat();
                }
            }
        }

        public static void Postfix(PlayerControl __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;
            if (CachedPlayer.LocalPlayer.PlayerControl == __instance)
            {
                setBasePlayerOutlines();

                updatePlayerInfo();

                sheriffSetTarget();

                impostorSetTarget();

                speedUpdate();
                GhostSpeedUpdate();
            }
        }
    }
}