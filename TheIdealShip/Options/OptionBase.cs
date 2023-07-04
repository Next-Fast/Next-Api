using System;
using TheIdealShip.Options.OptionValues;
using System.Collections.Generic;
using UnityEngine;

namespace TheIdealShip.Options;

public class OptionBase
{
    public string name;
    public int id;
    public OptionInfo optionInfo;
    public object optionValue;
    public Color nameColor;
    public optionTab tab;
    public optionType type;
    public OptionBase
    (
        string name,
        int id,
        object optionValue,
        optionTab tab,
        Color color,
        optionType type
    )
    {
        this.name = name;
        this.id = id;
        this.tab = tab;
        this.nameColor = color;
        this.optionValue = optionValue;
        this.type = type;

        optionInfo = new OptionInfo(name, id, this);
        OptionManager.AllOption.Add(this);
    }

    public void AddChildren(OptionInfo info) => optionInfo.AddChildren(info);
    public void RemoveChildren(OptionInfo info) => optionInfo.RemoveChildren(info);
    public void SetParent(OptionInfo info) => optionInfo.setParent(info);
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

public enum optionType
{
    Boolean,
    Float,
    String,
    Int
}