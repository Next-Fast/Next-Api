using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using NextShip.Api.Bases;
using NextShip.Api.Interfaces;
using NextShip.Options;
using NextShip.UI.Components;
using NextShip.UI.Module;

namespace NextShip.Manager;

public class NextOptionManager : INextOptionManager
{
    private readonly NextRoleManager _RoleManager;
    public readonly List<OptionBase> AllOptionBases = [];

    public NextOptionManager(NextRoleManager roleManager, EventManager eventManager)
    {
        _RoleManager = roleManager;

        eventManager.GetFastListener().Register(OnFastListener);
    }

    private void OnFastListener(FastEventArgs args)
    {
        if (args.EventName == "OptionCreate")
        {
            var _optionMenu = args.Get<NextOptionMenu>();
            var _MenuOption = args.Get<NextMenuOption>();
            DefOption.OptionCreate(this);
            _RoleManager.Roles.Do(n => n.OptionCreate(this));
        }
    }

    public void RegisterOptions(IEnumerable<OptionBase> options)
    {
        var optionBases = options.ToList();
        foreach (var option in optionBases) Register(option);
        AllOptionBases.AddRange(optionBases);
    }

    public void RegisterOption<T>(T option) where T : OptionBase
    {
    }

    private void Register(OptionBase option)
    {
    }

    public void GetOption<T>(int id) where T : OptionBase
    {
    }

    public void GetOption<T>(string Title) where T : OptionBase
    {
    }
}