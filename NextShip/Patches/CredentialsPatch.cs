using System.Text;
using HarmonyLib;
using Il2CppSystem;
using InnerNet;
using NextShip.Utilities;
using TMPro;
using UnityEngine;

//using static NextShip.Languages.LanguageCSV;

namespace NextShip.Patches;

[HarmonyPatch]
public static class CredentialsPatch
{
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
    public static class PingTrackerPatch
    {
        public static PingText pingText = new ();
        private static void Postfix(PingTracker __instance)
        {
            pingText.Update();
            
            ModManager.Instance.ShowModStamp();
            
            StringBuilder stringBuilder = new ();
            stringBuilder.AppendLine(__instance.text.text.ToColorString(pingText.GetPingColor()));
            if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
                stringBuilder.AppendLine($"<size=130%><color=#ff351f>The Ideal Ship \n - Next Ship</color></size> v{Main.Version.ToString()}\n");

            stringBuilder.AppendLine($"FPS: {pingText.GetFPS()}".ToColorString(pingText.GetFPSColor()));
            
            __instance.text.text = stringBuilder.ToString();
        }
    }

    public class PingText
    {
        private int frequency = 30;
        private int time;
        private float deltaTime;

        private int FPS;
        private int ping = AmongUsClient.Instance.Ping;
        private Color FPSColor;
        private Color PingColor;
        
        public PingText() {}

        public void Update()
        {
            deltaTime += Time.deltaTime;
            
            if (time == frequency)
            {
                FPS = (int)Mathf.Ceil(frequency / deltaTime);
                deltaTime = 0;
                time = 0;
            }
            
            time++;

            if (ping > 60) PingColor = Color.cyan;
            if (ping > 120) PingColor = Color.green;
            if (ping > 180) PingColor = Color.blue;
            if (ping > 240) PingColor = Color.white;
            if (ping > 300) PingColor = Color.white;
            if (ping > 500) PingColor = Color.red;

            if (FPS > 200) FPSColor = new Color32(255, 255, 0, Byte.MaxValue);
            if (FPS <= 200) FPSColor = new Color32(99, 184, 255,Byte.MaxValue);
            if (FPS <= 120) FPSColor = new Color32(205, 201, 201,Byte.MaxValue);
            if (FPS <= 90) FPSColor = new Color32(84, 255, 159, Byte.MaxValue);
            if (FPS <= 60) FPSColor = new Color32(240,248,255, Byte.MaxValue);
            if (FPS <= 30) FPSColor = new Color32(255, 222 ,173, Byte.MaxValue);
            if (FPS <= 15) FPSColor = Color.red;
        }

        public Color GetPingColor() => PingColor;
        public Color GetFPSColor() => FPSColor;
        public int GetFPS() => FPS;
        
    }
}