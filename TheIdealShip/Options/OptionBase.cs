using System.Collections.Generic;

namespace TheIdealShip.Options;

public class OptionBase : IOptionInfo
{
    public bool enable { get; set; }
    public string optionName { get; }
    public int optionId { get; }
    public int hierarchy { get; }
    public IOptionInfo parent { get; }
    public List<IOptionInfo> children { get; }
    public OptionBase
    (
        string name,
        int id = -1
    )
    {
        optionName = name;
        optionId = id == -1 ? OptionManager.AllOption.Count + 1 : id;
        OptionManager.AllOption.Add(this);
    }
}