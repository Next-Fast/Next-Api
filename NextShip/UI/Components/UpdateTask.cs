using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NextShip.Utilities.Attributes;
using UnityEngine;

namespace NextShip.UI.Components;

[Il2CppRegister]
public class UpdateTask : MonoBehaviour
{
    public static List<ShipTask> Tasks;
    public bool startd;

    public void Start()
    {
        Tasks = new List<ShipTask>();
        startd = true;
    }

    public void FixedUpdate()
    {
        if (!startd) return;
        if (Tasks == null) return;

        Tasks.Do(UpdateTaskTime);
        Tasks.Where(n => n.Priority == ShipTask.priority.High).Do(StartTask);
    }

    public void LateUpdate()
    {
        if (!startd) return;
        if (Tasks == null) return;

        Tasks.Do(StartTask);
    }

    private void StartTask(ShipTask task)
    {
        if (task.Time > 0) return;

        task.Task.Invoke();
        Tasks.Remove(task);
    }

    private void UpdateTaskTime(ShipTask Task)
    {
        if (Task.Time > 1)
            Task.Time -= 1;
        else
            Task.Time = 0;
    }
}

public class ShipTask
{
    public enum priority
    {
        High = 0,
        Medium = 1,
        Low = 2
    }

    public readonly priority Priority;
    public readonly Action Task;
    private readonly TaskState TaskState;
    public float Time;

    public ShipTask(float time, Action task, priority priority = priority.Low)
    {
        Task = task;
        Time = time;
        TaskState = new TaskState();
        Task += () => TaskState.Completed();
        Priority = priority;
    }

    public TaskStateEnum GetState => TaskState.Get();

    public void register()
    {
        UpdateTask.Tasks.Add(this);
    }
}

public class LastShipTask : ShipTask
{
    public LastShipTask(float time, Action task) : base(time, task, priority.High)
    {
    }
}