using HarmonyLib;
using InnerNet;
using NextShip.Utilities;
using TMPro;
using UnityEngine;
using static NextShip.Languages.Language;

//using static NextShip.Languages.LanguageCSV;

namespace NextShip.Patches;

[HarmonyPatch]
public static class CredentialsPatch
{
    public static string Credentials =
        @$"
        <size=130%><color=#ff351f>The Ideal Ship</color></size>v{Main.Version.ToString()}
        <size=60%><color=#a9e3ff>{GetString("Credential")}</color></size>
        ";

    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    private static class VersionShowerPatch
    {
        private static string stringText;

        private static void Postfix(VersionShower __instance)
        {
            stringText = __instance.text.text;
#if DEBUG
            stringText += " " + $"{ThisAssembly.Git.Branch} {ThisAssembly.Git.Commit}";
#endif

            stringText += "" + $"作者:天寸梦初  ver{Main.AmongUsVersion}";

            __instance.text.text = stringText;
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
                modstamp.layer = LayerMask.NameToLayer("UI");
                var rend = modstamp.AddComponent<SpriteRenderer>();
                rend.sprite = SpriteUtils.getModStamp();
                rend.color = new Color(1, 1, 1, 0.5f);
                modstamp.transform.localScale *= 0.6f;
                var offset = AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started ? 0.75f : 0f;
                modstamp.transform.position =
                    FastDestroyableSingleton<HudManager>.Instance.MapButton.transform.position + Vector3.down * offset;
            }
            else
            {
                ModManager.Instance.ShowModStamp();
            }
        }

        private static void Postfix(PingTracker __instance)
        {
            __instance.text.alignment = TextAlignmentOptions.TopRight;
            var text = Credentials;

            if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
                __instance.text.text =
                    $"<size=130%><color=#ff351f>The Ideal Ship</color></size> v{Main.Version.ToString()}\n" +
                    __instance.text.text;
            if (noGameEnd.getBool()) text += "\n" + GetString("NoGameEnd");
            __instance.text.text = text + __instance.text.text;
        }
    }
}