using UnityEngine;

namespace NextShip.Api.UI;

public class NextUI : IDisposable
{
    public string UIName { get; set; } = "NoName";

    public int IntId;

    public TaskStateEnum State;

    public NextUI()
    {
        State = TaskStateEnum.None;
        IntId = NextUIManager.Instance.AllCount;
        NextUIManager.Instance.Add(this);
    }
    
    public static NextUI StartUI(GameObject gameObject)
    {
        return new NextUI();
    }

    public void Update()
    {
        
    }
    
    public void Dispose()
    {
        State = TaskStateEnum.Completed;
    }
}