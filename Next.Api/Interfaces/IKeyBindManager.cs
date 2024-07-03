using Next.Api.Bases;

namespace Next.Api.Interfaces;

public interface IKeyBindManager
{
    public void AddBind(NKeyBind bind);

    public void RemoveBind(NKeyBind bind);
}