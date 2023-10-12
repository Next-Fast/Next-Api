using System.Collections.Generic;
using HarmonyLib;
using Il2CppSystem;
using TMPro;
using UnityEngine;

namespace NextShip.Chat.Patches;

[HarmonyPatch]
public static class ChatPatch
{
    private static GameObject CommandPromptBox;
    private static readonly List<GameObject> AllButton = new();
    public static string CurrentText;

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
    [HarmonyPostfix]
    public static void ChatController_Awake_Postfix(ChatController __instance)
    {
        __instance.freeChatField.OnChangedEvent += (Action)(() => OnChatChanged(__instance));
        new Command(new[] { "h" }, () => Info("h"));
        new Command(new[] { "abab", "text" }, () => Info("ababaa"));
    }


    private static void OnChatChanged(ChatController __instance)
    {
        InitCommandPromptBox(__instance);
        var text = __instance.freeChatField.Text;
        CurrentText = text;
        CommandPromptBox.SetActive(text.StartsWith("/"));
        if (text.StartsWith("/")) UpdateCommandPromptBox(GetText(text), __instance);
    }

    private static string[] GetText(string text)
    {
        var strings = new List<string>();
        var AllStrings = new List<string>();
        foreach (var c in text)
            switch (c)
            {
                case '/':
                    continue;
                case ' ':
                    AllStrings.Add(strings.ToText());
                    Info($"Add {strings.ToText()}");
                    strings = new List<string>();
                    continue;
                default:
                    Info($"add {c}");
                    strings.Add(c.ToStringText());
                    break;
            }

        Info($"GetString : {AllStrings.ToText()}");
        return AllStrings.ToArray();
    }

    private static void InitCommandPromptBox(ChatController __instance)
    {
        if (CommandPromptBox) return;

        CommandPromptBox = new GameObject("CommandPromptBox");
        CommandPromptBox.transform.SetParent(__instance.freeChatField.transform);
        CommandPromptBox.transform.localPosition = new Vector3(0.0031f, 0.559f, -1);
        var border = ObjetUtils.Find<Sprite>("smallButtonBorder");
        Sprite.Create(border.texture, new Rect(383, 272, 64, 64), new Vector2(0.5f, 0.5f), border.pixelsPerUnit, 0,
            SpriteMeshType.FullRect, border.border).CaChe("PromptBorder");

        var background = ObjetUtils.Find<Sprite>("blank");
        var SR = CommandPromptBox.AddComponent<SpriteRenderer>();
        SR.drawMode = SpriteDrawMode.Sliced;
        SR.sprite = background;
        SR.size = new Vector2(6.4f, 0.5f);
        SR.color = new Color(0.962f, 1, 1, 0.298f);
    }


    private static void UpdateCommandPromptBox(string[] text, ChatController __instance)
    {
        var BoxSR = CommandPromptBox.GetComponent<SpriteRenderer>();
        AllButton.Do(n => n.Destroy());
        var list = Command.GetCommands(text);
        if (list == null || list.Count == 0)
        {
            BoxSR.size = new Vector2(6.4f, 0.5f);
            return;
        }

        var count = 1;
        Vector3 vector3 = new(-0.0017f, -0.12f, -1);
        foreach (var command in Command.GetCommands(text))
        {
            if (count == 5) break;
            var button = new GameObject($"Button {count} : {text.ToText()}");
            button.transform.SetParent(CommandPromptBox.transform);
            var SR = button.AddComponent<SpriteRenderer>();
            SR.sprite = SpriteUtils.GetCache("PromptBorder");
            SR.drawMode = SpriteDrawMode.Sliced;
            SR.size = new Vector2(6.4f, 0.25f);
            SR.color = new Color(0, 0, 0, 0);
            button.transform.localPosition = vector3;
            ;
            vector3.y += 0.2f;

            var ButtonText = new GameObject("text");
            ButtonText.transform.SetParent(button.transform);
            ButtonText.transform.localPosition = new Vector3(6.85f, -2.34f, -1);
            var TMP = ButtonText.AddComponent<TextMeshPro>();
            TMP.text = command.GetText();
            TMP.fontSize = 3;
            TMP.color = new Color(0.962f, 1, 1, 0.298f);

            button.CreatePassiveButton(() => __instance.freeChatField.textArea.SetText(command.GetText()),
                OnMouseOut: () =>
                {
                    TMP.color = new Color(0.962f, 1, 1, 0.298f);
                    SR.color = new Color(0, 0, 0, 0);
                },
                OnMouseOver: () =>
                    SR.color = TMP.color = Palette.White
            );

            count++;
            AllButton.Add(button);
        }

        CommandPromptBox.AllGameObjectDo(n => n.layer = LayerUtils.GetUILayer());
    }
}