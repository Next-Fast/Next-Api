using System;
using System.Collections.Generic;
using HarmonyLib;
using NextShip.Utilities.Attributes;

namespace NextShip.Options;

public class OptionManager
{
    public static OptionManager _OptionManager;
    
    public static List<OptionBase> AllOption = new();
    public static List<OptionInfo> AllOptionInfo = new();
    public List<RoleOptionBase> AllRoleOption = new();
    public List<StringOptionBase> AllStringOption = new();
    public List<BooleanOptionBase> AllBooleanOption = new();
    public List<IntOptionBase> AllIntOption = new();
    public List<FloatOptionBase> AllFloatOption = new();

    public OptionManager()
    {
        if ((AllBooleanOption.Count | AllRoleOption.Count | AllStringOption.Count | AllIntOption.Count |
             AllFloatOption.Count) != 0) return;
        foreach (var optionBase in AllOption)
        {
            switch (optionBase.type)
            {
                case optionType.none:
                    continue;
                case optionType.Boolean:
                    AllBooleanOption.Add(optionBase as BooleanOptionBase);
                    break;
                case optionType.Float:
                    AllFloatOption.Add(optionBase as FloatOptionBase);                    
                    break;
                case optionType.String:
                    AllStringOption.Add(optionBase as StringOptionBase);
                    break;
                case optionType.Int:
                    AllIntOption.Add(optionBase as IntOptionBase);                    
                    break;
                case optionType.Role:
                    AllRoleOption.Add(optionBase as RoleOptionBase);                    
                    break;
                default:
                    continue;
            }
        }
    }

    public static OptionManager Get() => _OptionManager ?? new OptionManager();
    
    public static void Load()
    {
        OptionLoad.StartOptionLoad();
        Check();
    }

    public static void Check()
    {
        var e = new List<int>();
        AllOption.Do(check);

        void check(OptionBase @base)
        {
            if (e.Contains(@base.id))
                Warn($"选项id冲突 id: {@base.id} name: {@base.Title}");
            else
                e.Add(@base.id);
        }
    }
}