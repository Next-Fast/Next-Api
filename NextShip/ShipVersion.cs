using UnityEngine;

namespace NextShip;

public class ShipVersion
{   
    public int Major { get; protected set; }
    public int Minor { get; protected set; }
    public int Info { get; protected set; }
    
    public string StringText { get; protected set; }
    
    internal ShipVersion() { }

    public ShipVersion(int major, int minor, int info)
    {
        Major = major;
        Minor = minor;
        Info = info;
    }    

    public void set(int major, int minor, int info)
    {
        Major = major;
        Minor = minor;
        Info = info;
    }
}

public class AmongUsVersion : ShipVersion
{
    public int Year { get; protected set; }
    public int Month { get; protected set; }
    public int Day { get; protected set; }
    public int Revision { get; protected set; }

    public AmongUsVersion(int major, int minor, int info) : base(major, minor, info)
    {
    }
    
    internal AmongUsVersion() {}

    internal AmongUsVersion Form()
    {
        return new AmongUsVersion();
    }

    public AmongUsVersion AmongUsSet(int year, int month, int day, int revision = 0)
    {
        Year = year;
        Month = month;
        Day = day;
        Revision = revision;
        return this;
    }

    internal static AmongUsVersion GetFormConstants() =>
        new AmongUsVersion().AmongUsSet(Constants.Year, Constants.Month, Constants.Day, Constants.Revision);

    internal static string GetBuildVersion() => Application.version;

    public static int GetVersion(int year, int month, int day, int rev) =>
        year * 25000 + month * 1800 + day * 50 + rev;

    public static explicit operator string(AmongUsVersion Ver)
    {
        if (Ver.Major == 0 || Ver.Minor == 0 || Ver.Info == 0) return string.Empty;
        return $"{Ver.Major}.{Ver.Minor}.{Ver.Day}";
    }

    public static explicit operator int(AmongUsVersion Ver)
    {
        if (Ver.Year == 0 || Ver.Month == 0 || Ver.Day == 0) return 0;
        return GetVersion(Ver.Year, Ver.Month, Ver.Day, Ver.Revision);
    }
}