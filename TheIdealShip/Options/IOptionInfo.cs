using System.Collections.Generic;

namespace TheIdealShip.Options;

public interface IOptionInfo
{
    public bool enable { get; set; }
    public string optionName { get; }
    public int optionId { get; }
    public int hierarchy { get; }
    public IOptionInfo parent { get; }
    public List<IOptionInfo> children { get; }
}