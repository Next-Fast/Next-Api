namespace NextShip.Api.Interfaces;

public interface INextEvent
{
    public string EventName { get; set; }

    public void OnRegister(IEventManager @eventManager);

    public void OnUnRegister(IEventManager @eventManager);
}