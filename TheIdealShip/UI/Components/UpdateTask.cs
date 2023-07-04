using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using TheIdealShip.Utilities.Attribute;

namespace TheIdealShip.UI.Components;

[Il2CppRegister]
public class UpdateTask : MonoBehaviour
{
    public float time;
    public Action task;
    public static List<UpdateTask> Tasks = new();

    public UpdateTask()
    {
    }

    public void Start()
    {

    }

    public void Awake()
    {
    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {
        if (Tasks != null)
        {
            Tasks.Do(UpdatTaskTime);
        }
    }

    public void UpdatTaskTime(UpdateTask task)
    {
        if (time > 1)
            task.time -= 1;
        else
            task.time = 0;
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
                return;
        }
    }
}