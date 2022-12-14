using UnityEngine;
using HarmonyLib;


namespace TheIdealShip.Patches
{
    [HarmonyPatch]
    public static class CredentialsPatch
    {
        public static void showmodstamp()
        {
            ModManager.Instance.ShowModStamp();
        }
        public static string Credentials =
        $@"
        <size=130%><color=#ff351f>The Ideal Ship</color></size>v{TheIdealShipPlugin.Version.ToString()}
        <size=60%><color=#a9e3ff>Modded : MC-AS-Huier</color></size>
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
                credentials.SetText($"v{TheIdealShipPlugin.Version.ToString()}\nModded : MC-AS-Huier<size=30f%>");
                credentials.alignment = TMPro.TextAlignmentOptions.Center;
                credentials.fontSize *= 0.75f;

                credentials.transform.SetParent(amongUsLogo.transform);
            }
        }

        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        internal static class PingTrackerPatch
        {

            static void Postfix(PingTracker __instance)
            {
                __instance.text.alignment = TMPro.TextAlignmentOptions.TopRight;
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
                {
                    __instance.text.text = $"<size=130%><color=#ff351f>The Ideal Ship</color></size> v{TheIdealShipPlugin.Version.ToString()}\n" + __instance.text.text;
                }
                else
                {
                    __instance.text.text = $"{Credentials}\n{__instance.text.text}";
                    __instance.transform.localPosition = new Vector3(3.5f, __instance.transform.localPosition.y, __instance.transform.localPosition.z);
                }
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
