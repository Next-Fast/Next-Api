using System;
using System.Collections.Generic;
using HarmonyLib;
using NextShip.Utilities.Attributes;
using UnityEngine;

namespace NextShip.UI.Components;

[Il2CppRegister]
public class UpdateTask : MonoBehaviour
{
    public List<ShipTask> Tasks;
    

    public void Start()
    {
        Tasks = new List<ShipTask>();
    }

    public void FixedUpdate()
    {
        if (Tasks != null) Tasks.Do(UpdatTaskTime);
    }

    public void LateUpdate()
    {
        Tasks.Do(StartTask);

        void StartTask(ShipTask task)
        {
            if (task.Time > 0) return;
            
            task.Task.Invoke();
            Tasks.Remove(task);
        }
    }

    public void UpdatTaskTime(ShipTask Task)
    {
        if (Task.Time > 1)
            Task.Time -= 1;
        else
            Task.Time = 0;
    }
}

public class ShipTask
{
    public float Time;
    public Action Task;
    public ShipTask(float time,Action task)
    {
        Task = task;
        Time = time;
    }
}

public class LastShipTask : ShipTask
{
    public LastShipTask(float time, Action task) : base(time, task)
    {
    }
}