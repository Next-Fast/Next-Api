using System.Collections.Generic;
using NextShip.Utilities.Attributes;

namespace NextShip.Options;

public static class OptionManager
{
    public static List<OptionBase> AllOption = new();
    public static List<OptionInfo> AllOptionInfo = new();
    public static List<RoleOptionBase> AllRoleOption = new List<RoleOptionBase>();
    public static List<StringOptionBase> AllStringOption = new List<StringOptionBase>();
    public static List<BooleanOptionBase> AllBooleanOption = new List<BooleanOptionBase>();

    public static void Load()
    {
        OptionLoad.StartOptionLoad();
    }
}