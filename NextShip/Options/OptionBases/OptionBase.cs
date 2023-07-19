using System.Collections.Generic;
using UnityEngine;

namespace NextShip.Options;

public abstract class OptionBase
{
    public int id;
    public string StringId;
    public string Title;
    public Color nameColor;
    public OptionInfo optionInfo;
    public optionTab tab;
    public optionType type;
    public OptionBehaviour OptionBehaviour;
    public bool EnableTranslation;


    public OptionBase
    (
        string Title,
        int id,
        optionTab tab,
        optionType type,
        bool Translation = true
    )
    {
        this.Title = Title;
        this.id = id;
        this.tab = tab;
        this.type = type;
        EnableTranslation = Translation;

        optionInfo = new OptionInfo(Title, StringId,id, this);
        OptionManager.AllOption.Add(this);
    }

    public OptionBase
    (
        string Title,
        string stringId,
        optionTab tab,
        optionType type,
        bool Translation = true
    )
    {
        this.Title = Title;
        this.tab = tab;
        this.type = type;
        StringId = stringId;
        EnableTranslation = Translation;

        optionInfo = new OptionInfo(Title, StringId, id, this);
        OptionManager.AllOption.Add(this);
    }

    public void SetId(int Id = -1, string stringId = "")
    {
        id = Id;
        StringId = stringId;
    }

    public void AddChildren(OptionInfo info)
    {
        optionInfo.AddChildren(info);
    }

    public void RemoveChildren(OptionInfo info)
    {
        optionInfo.RemoveChildren(info);
    }

    public void SetParent(OptionInfo info)
    {
        optionInfo.setParent(info);
    }

    public abstract void Increase();

    public abstract void Decrease();
    public abstract int GetInt();
    public abstract float GetFloat();
    public abstract string GetValueString();
    public abstract OptionBase GetBase();

    public string GetTitleString()
    {
        if (EnableTranslation) return GetString(Title);
        return Title;
    }
}

public class OptionInfo : IOptionInfo
{
    public OptionInfo
    (
        string Name,
        string stringId,
        int Id,
        OptionBase optionBase,
        int Hierarchy = 0,
        OptionInfo Parent = null,
        List<OptionInfo> Children = null
    )
    {
        optionName = Name;
        this.stringId = stringId;
        optionId = Id;
        option = optionBase;
        hierarchy = Hierarchy;
        parent = Parent;
        children = Children;
        OptionManager.AllOptionInfo.Add(this);
    }

    public bool enable { get; set; }
    public string optionName { get; }
    public string stringId { get; }
    public int optionId { get; }
    public int hierarchy { get; }
    public OptionInfo parent { get; set; }
    public List<OptionInfo> children { get; set; }
    public OptionBase option { get; }

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
    none,
    Boolean,
    Float,
    String,
    Int
}