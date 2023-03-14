using UnityEngine;
using HarmonyLib;
using TheIdealShip.Utilities;
using static TheIdealShip.Languages.Language;
//using static TheIdealShip.Languages.LanguageCSV;

namespace TheIdealShip.Patches
{
    [HarmonyPatch]
    public static class CredentialsPatch
    {
        public static string Credentials =
        @$"
        <size=130%><color=#ff351f>The Ideal Ship</color></size>v{TheIdealShipPlugin.Version.ToString()}
        <size=60%><color=#a9e3ff>{GetString("Credential")}</color></size>
        ";

        [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
        private static class VersionShowerPatch
        {
            static void Postfix(VersionShower __instance)
            {
                var amongUsLogo = GameObject.Find("bannerLogo_AmongUs");
                if (amongUsLogo == null) return;

                var credentials = UnityEngine.Object.Instantiate<TMPro.TextMeshPro>(__instance.text);
                credentials.transform.position = new Vector3(0, 0, 0);
                credentials.SetText($"v{TheIdealShipPlugin.Version.ToString()}\n{GetString("Credential")}<size=30f%>");
                credentials.alignment = TMPro.TextAlignmentOptions.Center;
                credentials.fontSize *= 0.75f;

                credentials.transform.SetParent(amongUsLogo.transform);
            }
        }

        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        internal static class PingTrackerPatch
        {
            public static void modlogo()
            {
                var modstamp = GameObject.Find("ModStamp");
                if (modstamp != null)
                {
                    modstamp.layer = UnityEngine.LayerMask.NameToLayer("UI");
                    var rend = modstamp.AddComponent<SpriteRenderer>();
                    rend.sprite = Helpers.getModStamp();
                    rend.color = new Color(1, 1, 1, 0.5f);
                    modstamp.transform.localScale *= 0.6f;
                    float offset = (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started) ? 0.75f : 0f;
                    modstamp.transform.position = FastDestroyableSingleton<HudManager>.Instance.MapButton.transform.position + Vector3.down * offset;
                }
                else
                {
                    ModManager.Instance.ShowModStamp();
                }
            }

            static void Postfix(PingTracker __instance)
            {
                __instance.text.alignment = TMPro.TextAlignmentOptions.TopRight;
                string text = Credentials;
/*
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
                {
                    __instance.text.text = $"<size=130%><color=#ff351f>The Ideal Ship</color></size> v{TheIdealShipPlugin.Version.ToString()}\n"+ __instance.text.text;
                }
*/
/*
                if (CustomOptionHolder.noGameEnd.getBool())
                {
                    text += "\n" + GetString("NoGameEnd").ToColorString(colo);
                } */
                __instance.text.text = text + __instance.text.text;
            }
        }

        [HarmonyPatch(typeof(MainMenuManager),nameof(MainMenuManager.Start))]
        public static class LogoPatch
        {
                static void Postfix(PingTracker __instance)
                {
                var amongUsLogo = GameObject.Find("bannerLogo_AmongUs");
                if (amongUsLogo != null)
                {
                    amongUsLogo.transform.localScale *= 0.6f;
                    amongUsLogo.transform.position += Vector3.up* 0.25f;
                }

                var Logo = new GameObject("bannerLogo_TheIdealShip");
                Logo.transform.position = Vector3.up;
                var renderer = Logo.AddComponent<SpriteRenderer>();
                renderer.sprite = Helpers.LoadSpriteFromResources("TheIdealShip.Resources.Banner.png", 300f);
            }
        }
    }
}
