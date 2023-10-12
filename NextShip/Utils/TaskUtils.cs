using System;
using System.Threading.Tasks;
using HarmonyLib;

namespace NextShip.Utils;

public static class TaskUtils
{
    public static void StartTask(Action[] actions)
    {
        actions.Do(
            StartTask);
    }

    public static void StartTask(Action action)
    {
        var task = new Task(action);
        task.Start();
        Info($"TaskUtils : {action.Method.Name}");
    }
}