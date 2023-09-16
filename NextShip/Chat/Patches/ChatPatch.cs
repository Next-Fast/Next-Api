using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace NextShip.Chat.Patches;

[HarmonyPatch]
public static class ChatPatch
{
    private static GameObject CommandPromptBox;
    private static List<GameObject> AllButton = new ();
    
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake)), HarmonyPostfix]
    public static void ChatController_Awake_Postfix(ChatController __instance)
    {
        __instance.freeChatField.OnChangedEvent +=  (Il2CppSystem.Action)(() => OnChatChanged(__instance));
        var command = new Command(new[] { "h" }, () => Info("h"));
    }


    private static void OnChatChanged(ChatController __instance)
    {
        InitCommandPromptBox(__instance);
        var text = __instance.freeChatField.Text;
        CommandPromptBox.SetActive(text.StartsWith("/"));
        if (text.StartsWith("/")) UpdateCommandPromptBox(text.Replace("/", "").Split(""), __instance);
    }

    private static void InitCommandPromptBox(ChatController __instance)
    {
        if (CommandPromptBox) return;
        
        CommandPromptBox = new GameObject("CommandPromptBox");
        CommandPromptBox.transform.SetParent(__instance.freeChatField.transform);
        CommandPromptBox.transform.localPosition = new Vector3(0.0031f, 0.559f, -1);
        
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
        var list = Command.GetCommands(text);
        if (list == null || list.Count == 0)
        {
            BoxSR.size = new Vector2(6.4f, 0.5f);
            AllButton.Do(n => n.Destroy());
            return;
        }

        var buttonSprite = ObjetUtils.Find<Sprite>("smallButtonBorder");
        var count = 1;
        Vector3 vector3 = new(-0.0017f, -0.12f, -1);
        foreach (var command in Command.GetCommands(text))
        {
            if (count == 5) break;
            var button = new GameObject($"Button {text[0]}");
            button.transform.SetParent(CommandPromptBox.transform);
            var SR = button.AddComponent<SpriteRenderer>();
            SR.sprite = buttonSprite;
            SR.drawMode = SpriteDrawMode.Sliced;
            SR.size = new Vector2(6.4f, 0.25f);
            button.transform.localPosition = vector3;;
            vector3.y += 0.2f;

            var ButtonText = new GameObject("text");
            ButtonText.transform.SetParent(button.transform);
            ButtonText.transform.localPosition = new Vector3(6.85f, -2.34f, -1);
            var TMP = ButtonText.AddComponent<TextMeshPro>();
            TMP.text = command.GetText();
            TMP.fontSize = 3;
            TMP.color = new Color(0.962f, 1, 1, 0.298f);
            
            button.CreatePassiveButton(onClick: () => __instance.freeChatField.textArea.SetText(command.GetText()), 
                OnMouseOut: () =>
            {
                TMP.color = new Color(0.962f, 1, 1, 0.298f);
                SR.color = new Color(0, 0, 0, 0);
            }, 
                OnMouseOver: () => 
                    SR.color = TMP.color = Palette.White
                    );

            BoxSR.size = new Vector2(6.4f, count * 0.08f); 
            count++;
            AllButton.Add(button);
        }
        
        CommandPromptBox.AllGameObjectDo(n => n.layer = LayerUtils.GetUILayer());
    }
}

public class Command
{
    public static readonly List<Command> AllCommand;

    public string[] key;
    public Action CommandEvent;
    public int count;
    private int KeyCount => key.Length;

    static Command()
    {
        AllCommand = new List<Command>();
    }

    public Command(string[] key, Action CommandEvent)
    {
        this.CommandEvent = CommandEvent;
        this.key = key;
        count = AllCommand == null ? 0 : AllCommand.Last().count++;
        
        AllCommand!.Add(this);
    }

    public string GetText()
    {
        var text = "/";
        foreach (var k in key)
        {
            text += k;
            text += " ";
        }

        return text;
    }

    private bool Compare(IReadOnlyCollection<string> keys)
    {
        if (keys.Count > key.Length) return false;
        if (keys == key) return true;

        var Is = true;
        var Count = 0;
        foreach (var k in keys)
        {
            var ThisKey = key[Count];
            if (ThisKey != k)
            {
                Is = false; 
                break;
            }
            
            Count++;
        }

        return Is;
    }

    public static List<Command> GetCommands(string[] keys)
    {
        var Commands = AllCommand.Where(n => n.Compare(keys)).ToList();
        Commands.Sort((x, y) => x.KeyCount.CompareTo(y.KeyCount));
        /*if (Commands.First().KeyCount > Commands.Last().KeyCount) Commands.Reverse();*/

        return Commands;
    }
}