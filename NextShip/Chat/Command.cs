using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NextShip.Chat.Patches;

namespace NextShip.Chat;

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
        count = AllCommand.Count;
        this.key = key;
        
        AllCommand!.Add(this);
    }

    public string GetText()
    {
        var text = "/";
        foreach (var k in key)
        {
            text += k;
            if (k == key.Last()) continue;
            text += " ";
        }

        return text;
    }

    public static bool TryGetCommandEvent(string text,out Action action)
    {
        action = null;
        var command = AllCommand.Find(n => n.Compare(text));
        if (command == null) return false;
        action = command.CommandEvent;
        return true;        
    }
    
    private bool Compare(string text)
    {
        return GetText() == text;
    }

    private bool Compare(IReadOnlyCollection<string> keys)
    {
        if (ChatPatch.CurrentText == "/") return true;
        Info($"compare {keys.ToText()}");
        if (keys.Count > key.Length) return false;
        Info("keys.Count > key.Length");
        if (keys == key) return false;
        Info("keys == key");
        if (GetText() == ChatPatch.CurrentText) return false;
        Info("GetText() == ChatPatch.CurrentText");

        var Is = true;
        for (var i = 0; i < keys.Count; i++)
        {
            var TKey = keys.ElementAt(i);
            var VKey = key.ElementAt(i);
            if (TKey != VKey) Is = Compare(TKey, VKey);
            if (!Is) break;
        }

        return Is;
    }

    private static bool Compare(string TKey, string VKey)
    {
        Info($"{"Compare: " + TKey + " - " + VKey}");
        return VKey.Contains(TKey);
    }
    
    public static List<Command> GetCommands(string[] keys)
    {
        var Commands = AllCommand.Where(n => n.Compare(keys)).ToList();
        Commands.Sort((x, y) => x.KeyCount.CompareTo(y.KeyCount));
        /*if (Commands.First().KeyCount > Commands.Last().KeyCount) Commands.Reverse();*/

        return Commands;
    }
}