using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace NextShip.Chat.Patches;

[HarmonyPatch]
public static class ChatPatch
{
    private static GameObject CommandPromptBox;
    
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
        if (text.StartsWith("/")) UpdateCommandPromptBox(text.Remove(1).Split(" "));
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

    private static void UpdateCommandPromptBox(string[] text)
    {
        foreach (var command in Command.GetCommands(text))
        {
            var button = new GameObject(command.key.ToString());
            button.transform.SetParent(CommandPromptBox.transform);
            var SR = button.AddComponent<SpriteRenderer>();
            SR.sprite = ObjetUtils.Find<Sprite>("smallButtonBorder");
            SR.drawMode = SpriteDrawMode.Sliced;
            button.CreatePassiveButton(OnMouseOut: () => SR.color = new Color(0.962f, 1, 1, 0.298f), OnMouseOver: () => SR.color = Palette.White);

            var ButtonText = new GameObject("text");
            ButtonText.transform.SetParent(button.transform);
            var TMP = ButtonText.AddComponent<TextMeshPro>();
            TMP.text = command.GetText();

        }
        
        CommandPromptBox.AllGameObjectDo(n => n.layer = LayerUtils.GetUILayer());
    }
}

public class Command
{
    public static readonly List<Command> AllCommand;

    public string[] key;
    public Action CommandEvent;
    private int KeyCount => key.Length;

    static Command()
    {
        AllCommand = new List<Command>();
    }

    public Command(string[] key, Action CommandEvent)
    {
        this.CommandEvent = CommandEvent;
        this.key = key;
        
        AllCommand.Add(this);
    }

    public string GetText()
    {
        var text = "/ ";
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
        if (Commands.First().KeyCount > Commands.Last().KeyCount) Commands.Reverse();

        return Commands;
    }
}