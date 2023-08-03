using System.Collections.Generic;
using HarmonyLib;
using NextShip.Utilities.Attributes;

namespace NextShip.Options;

public static class OptionManager
{
    public static List<OptionBase> AllOption = new();
    public static List<OptionInfo> AllOptionInfo = new();
    public static List<RoleOptionBase> AllRoleOption = new ();
    public static List<StringOptionBase> AllStringOption = new ();
    public static List<BooleanOptionBase> AllBooleanOption = new ();
    public static List<IntOptionBase> AllIntOption = new();
    public static List<FloatOptionBase> AllFloatOption = new();
    
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
            {
                Warn($"选项id冲突 id: {@base.id} name: {@base.Title}");
            }
            else
            {
                e.Add(@base.id);
            }
        }
    }
}