namespace NextShip.Api.Interfaces;

public interface IEventManager
{
    // 注册事件
    public void RegisterEvent(INextEvent @event);
    
    
    public void UnRegisterEvent(INextEvent @event);
}