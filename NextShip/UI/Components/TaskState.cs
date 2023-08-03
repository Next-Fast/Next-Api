using System;

namespace NextShip;

public class TaskState
{
    private TaskStateEnum StateEnum;
    private Action Action;
    public virtual TaskStateEnum Get() => StateEnum;
    public TaskState()
    {
        StateEnum = TaskStateEnum.None;
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