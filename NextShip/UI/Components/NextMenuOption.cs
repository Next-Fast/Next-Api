using NextShip.UI.Module;
using UnityEngine;

namespace NextShip.UI.Components;

[Il2CppRegister]
public class NextMenuOption : MonoBehaviour
{
    public Transform List;
    public bool Open;
    public NextOptionMenu __OptionMenu;

    public void Start()
    {
        _eventManager.GetFastListener().Call("OptionCreate", [__OptionMenu, this]);
        CreateOption();
    }


    public void OnDestroy()
    {
        NextOptionMenu.Instance.Initd = false;
        NextOptionMenu.Instance = null;
    }

    public void CreateOption()
    {
    }

    public void CreateOptionButton()
    {
    }
}