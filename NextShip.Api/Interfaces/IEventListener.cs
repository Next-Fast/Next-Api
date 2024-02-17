namespace NextShip.Api.Interfaces;

public interface IEventListener
{
    public void On(string name) {}

    public void On(INextEvent @event) {}
}