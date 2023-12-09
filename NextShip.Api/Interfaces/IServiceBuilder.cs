using Microsoft.Extensions.DependencyInjection;

namespace NextShip.Api.Interfaces;

public interface IServiceBuilder
{
    public IServiceBuilder CreateService();

    public IServiceBuilder Add<T>() where T : class;

    public IServiceBuilder Add(Type type);

    public INextService Build();

    public IServiceBuilder Set(ServiceCollection collection);
}