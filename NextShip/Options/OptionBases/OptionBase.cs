using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NextShip.Options;

public abstract class OptionBase
{
    public bool EnableTranslation;
    public int id;
    public Color nameColor;
    public OptionBehaviour OptionBehaviour;
    public OptionInfo optionInfo;
    public string StringId;
    public optionTab tab;
    public string Title;
    public optionType type;


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
        this.id = GetId(id);
        this.tab = tab;
        this.type = type;
        EnableTranslation = Translation;

        optionInfo = new OptionInfo(Title, string.Empty, id, this);
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

    public int GetId(int Id)
    {
        if (Id != -1) return id;

        var optionid = 0;
        while (OptionManager.AllOption.FirstOrDefault(n => n.id == optionid) != null) optionid += 1;

        return optionid;
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

    // 设置OptionBehaviour隐性转换
    public static explicit operator OptionBehaviour(OptionBase @base)
    {
        return @base.OptionBehaviour;
    }

    public static explicit operator string(OptionBase @base)
    {
        return @base.GetValueString();
    }


    public string GetTitleString()
    {
        return EnableTranslation ? GetString(Title) : Title;
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

    public string stringId { get; }

    public bool enable { get; set; }
    public string optionName { get; }
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
    Int,
    Role
}