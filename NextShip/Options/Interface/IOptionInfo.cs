using System.Collections.Generic;

namespace NextShip.Options;

public interface IOptionInfo
{
    public bool enable { get; set; }
    public string optionName { get; }
    public int optionId { get; }
    public int hierarchy { get; }
    public OptionInfo parent { get; set; }
    public List<OptionInfo> children { get; set; }
    public OptionBase option { get; }
}