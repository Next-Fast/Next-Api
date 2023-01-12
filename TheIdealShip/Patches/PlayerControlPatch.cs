using HarmonyLib;
using TheIdealShip.Roles;
using TheIdealShip.Utilities;
using TMPro;
using UnityEngine;

namespace TheIdealShip.Patches
{
    [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.FixedUpdate))]
    public static class PlayerControlFixedUpdatePatch
    {
        public static void updatePlayerInfo()
        {
            foreach (PlayerControl p in CachedPlayer.AllPlayers)
            {
                if(p == CachedPlayer.LocalPlayer.PlayerControl ||CachedPlayer.LocalPlayer.Data.IsDead)
                {
                    Transform playerInfoTransform = p.cosmetics.nameText.transform.parent.FindChild("Info");
                    TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                    if (playerInfo == null)
                    {
                        playerInfo = UnityEngine.Object.Instantiate(p.cosmetics.nameText, p.cosmetics.nameText.transform.parent);
                        playerInfo.transform.localPosition += Vector3.up * 0.225f;
                        playerInfo.fontSize *= 0.75f;
                        playerInfo.gameObject.name = "Info";
                        playerInfo.color = playerInfo.color.SetAlpha(1f);
                    }

                    string roleNames = RoleInfo.GetRolesString(p, true, false);

                    string playerInfoText = "";
                    if (p == CachedPlayer.LocalPlayer.PlayerControl)
                    {
                        playerInfoText = $"{roleNames}";
                    }

                    playerInfo.text = playerInfoText;
                    playerInfo.gameObject.SetActive(p.Visible);
                }
            }
        }

        public static void Postfix(PlayerControl __instance)
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;
            if (CachedPlayer.LocalPlayer.PlayerControl == __instance)
            {
                updatePlayerInfo();
            }
        }
    }
}