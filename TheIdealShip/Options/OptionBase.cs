using System.Collections.Generic;
using UnityEngine;

namespace TheIdealShip.Options;

public class OptionBase
{
    public string name;
    public int id;
    public OptionInfo optionInfo;
    public OptionValue optionValue;
    public Color nameColor;
    public optionTab tab;
    public OptionBase
    (
        string name,
        int id,
        OptionValue optionValue,
        optionTab tab,
        Color color
    )
    {
        this.name = name;
        this.id = id;
        this.tab = tab;
        this.nameColor = color;
        this.optionValue = optionValue;

        optionInfo = new OptionInfo(name, id, this);
        OptionManager.AllOption.Add(this);
    }

    public void AddChildren(OptionInfo info) => optionInfo.AddChildren(info);
    public void RemoveChildren(OptionInfo info) => optionInfo.RemoveChildren(info);
    public void SetParent(OptionInfo info) => optionInfo.setParent(info);
}

public class OptionValue
{
    public int defaultValue;
    public int Value;
    public int min;
    public int step;
    public int max;
    public OptionValue
    (
        int defaultValue,
        int min,
        int step,
        int max
    )
    {
        this.defaultValue = defaultValue;
        this.min = min;
        this.step = step;
        this.max = max;

        Value = defaultValue;
    }

    public int GetValue() => Value;
    public void increase() => Value += step;
    public void decrease() => Value -= step;
}

public class OptionInfo : IOptionInfo
{
    public bool enable { get; set; }
    public string optionName { get; }
    public int optionId { get; }
    public int hierarchy { get; }
    public OptionInfo parent { get; set; }
    public List<OptionInfo> children { get; set; }
    public OptionBase option { get; }

    public OptionInfo
    (
        string Name,
        int Id,
        OptionBase optionBase,
        int Hierarchy = 0,
        OptionInfo Parent = null,
        List<OptionInfo> Children = null
    )
    {
        optionName = Name;
        optionId = Id;
        option = optionBase;
        hierarchy = Hierarchy;
        parent = Parent;
        children = Children;
        OptionManager.AllOptionInfo.Add(this);
    }

    public void setParent(OptionInfo optionInfo)
    {
        parent = optionInfo;
    }

    public void AddChildren(OptionInfo optionInfo)
    {
        children.Add(optionInfo);
    }

    public void RemoveChildren(OptionInfo optionInfo)
    {
        children.Remove(optionInfo);
    }
}

public enum optionTab
{
    GameSettings,
    Impostor,
    Crewmate,
    Neutral,
    other
}