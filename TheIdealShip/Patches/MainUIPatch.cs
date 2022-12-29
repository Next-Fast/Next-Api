using System;
using HarmonyLib;
using UnityEngine;
using TheIdealShip.Modules;

namespace TheIdealShip.Patches
{
    [HarmonyPatch]
    public static class MainUIPatch
    {
        public static GameObject Template;
        public static GameObject DiscordButton;
        public static GameObject kookButton;
        public static GameObject UpdateButton;

        [HarmonyPatch(typeof(MainMenuManager),nameof(MainMenuManager.Start)),HarmonyPrefix]
        public static void Start_Prefix(MainMenuManager __instance)
        {
            // 将玩法说明改为Github
            var howtoplayButton = GameObject.Find("HowToPlayButton");
            if (howtoplayButton != null)
            {
                var githubText = howtoplayButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
               // Color githubColor = new Color32(0,0,0,byte.MaxValue);
                howtoplayButton.GetComponent<PassiveButton>().OnClick = new();
                howtoplayButton.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() => Application.OpenURL(TheIdealShipPlugin.GithubURL)));
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => howtoplayButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText("Github"))));
              //  howtoplayButton.GetComponent<SpriteRenderer>().color = githubText.color = githubColor;
            }

            // 将自由模式改成bilibili
            var freeplayButton = GameObject.Find("FreePlayButton");
            if (freeplayButton != null)
            {
                var bilibiliText = freeplayButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                Color bilibiliColor = new Color32(0,182,247,byte.MaxValue);
                freeplayButton.GetComponent<PassiveButton>().OnClick = new();
                freeplayButton.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() => Application.OpenURL(TheIdealShipPlugin.bilibiliURL)));
                freeplayButton.GetComponent<PassiveButton>().OnMouseOut.AddListener((Action)(() => freeplayButton.GetComponent<SpriteRenderer>().color = bilibiliText.color = bilibiliColor));
                __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => freeplayButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().SetText("BiliBili"))));
                freeplayButton.GetComponent<SpriteRenderer>().color = bilibiliText.color = bilibiliColor;
            }


            if (Template == null) Template = GameObject.Find("ExitGameButton");
            if (Template == null) return;

            // 生成Discord按钮
            if (DiscordButton == null) DiscordButton = UnityEngine.Object.Instantiate(Template,Template.transform.parent);
            DiscordButton.name = "DiscordButton";
            DiscordButton.transform.position = Vector3.Reflect(Template.transform.position,Vector3.left);

            var DiscordText = DiscordButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            Color DiscordColor = new Color32(88,101,242,byte.MaxValue);
            PassiveButton DiscordPassiveButton = DiscordButton.GetComponent<PassiveButton>();
            SpriteRenderer DiscordButtonSprite = DiscordButton.GetComponent<SpriteRenderer>();
            DiscordPassiveButton.OnClick =new();
            DiscordPassiveButton.OnClick.AddListener((Action)(() => Application.OpenURL(TheIdealShipPlugin.DiscordURL)));
            DiscordPassiveButton.OnMouseOut.AddListener((Action)(() => DiscordButtonSprite.color = DiscordText.color = DiscordColor));
            __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => DiscordText.SetText("Discord"))));
            DiscordButtonSprite.color = DiscordText.color = DiscordColor;
            DiscordButton.gameObject.SetActive(true);

            // 生成Kook按钮
            if (kookButton == null) kookButton = UnityEngine.Object.Instantiate(Template,Template.transform.parent);
            kookButton.name = "KooKButton";
            kookButton.transform.position = DiscordButton.transform.position + new Vector3(0, 0.65f);

            var kookText = kookButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            Color kookColor = new Color32(122, 204, 53, byte.MaxValue);
            PassiveButton kookPassiveButton = kookButton.GetComponent<PassiveButton>();
            SpriteRenderer kookButtonSprite = kookButton.GetComponent<SpriteRenderer>();
            kookPassiveButton.OnClick = new();
            kookPassiveButton.OnClick.AddListener((Action)(() => Application.OpenURL(TheIdealShipPlugin.KOOKURL)));
            kookPassiveButton.OnMouseOut.AddListener((Action)(() => kookButtonSprite.color = kookText.color = kookColor));
            __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) => kookText.SetText("KooK"))));
            kookButtonSprite.color = kookText.color = kookColor;
            kookButton.gameObject.SetActive(true);

            // 生成Update按钮
            if (UpdateButton == null) UpdateButton = UnityEngine.Object.Instantiate(Template,Template.transform.parent);
            UpdateButton.name = "UpdateButton";
            UpdateButton.transform.position = Template.transform.position + new Vector3(0.25f, 0.75f);
            UpdateButton.transform.GetChild(0).GetComponent<RectTransform>().localScale *= 1.5f;

            var UpdateText = UpdateButton.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            Color UpdateColor = new Color32(128, 255, 255, byte.MaxValue);
            PassiveButton UpdatePassiveButton = UpdateButton.GetComponent<PassiveButton>();
            SpriteRenderer UpdateButtonSprite = UpdateButton.GetComponent<SpriteRenderer>();
            UpdatePassiveButton.OnClick = new();
            UpdatePassiveButton.OnClick.AddListener((Action)(() =>
            {
                UpdateButton.SetActive(false);
                ModUpdate.UpdateMod();
            }));
            UpdatePassiveButton.OnMouseOut.AddListener((Action)(() => UpdateButtonSprite.color = UpdateText.color = UpdateColor));
            UpdateButtonSprite.color = UpdateText.color = UpdateColor;
            UpdateButtonSprite.size *= 1.5f;
            UpdateButton.SetActive(ModUpdate.HUpdate);
        }
    }
}