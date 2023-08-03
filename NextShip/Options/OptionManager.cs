using System.Collections.Generic;
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
    }
    
    
}