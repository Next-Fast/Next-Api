using System;

namespace NextShip.UI.Components;

public class TaskState
{
    private Action Action;
    private TaskStateEnum StateEnum;

    public TaskState()
    {
        StateEnum = TaskStateEnum.None;
    }

    public virtual TaskStateEnum Get()
    {
        return StateEnum;
    }

    public void set(TaskStateEnum @enum)
    {
        StateEnum = @enum;
    }

    public void Completed()
    {
        StateEnum = TaskStateEnum.Completed;
        Action();
    }

    public void AddAction(Action action)
    {
        Action += action;
    }

    public void RemoverAction(Action action)
    {
        Action -= action;
    }
}

public enum TaskStateEnum
{
    None,
    Waiting,
    Processing,
    Completed,
    Failed
}