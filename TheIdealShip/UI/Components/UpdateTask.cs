using System;
using System.Collections.Generic;
using HarmonyLib;
using TheIdealShip.Utilities.Attributes;
using UnityEngine;

namespace TheIdealShip.UI.Components;

[Il2CppRegister]
public class UpdateTask : MonoBehaviour
{
    public static List<UpdateTask> Tasks = new();
    public float time;
    public Action task;

    public void Awake()
    {
    }

    public void Start()
    {
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
        if (Tasks != null) Tasks.Do(UpdatTaskTime);
    }

    public void LateUpdate()
    {
        Tasks.Do(StartTask);

        void StartTask(UpdateTask task)
        {
            if (task.time <= 0)
            {
                task.task.Invoke();
                Tasks.Remove(task);
            }
            else
            {
            }
        }
    }

    public void UpdatTaskTime(UpdateTask task)
    {
        if (time > 1)
            task.time -= 1;
        else
            task.time = 0;
    }
}