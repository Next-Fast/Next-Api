using NextShip.Api.Attributes;
using NextShip.Options;
using UnityEngine;

namespace NextShip.UI.Components;

[Il2CppRegister]
public class NextMenuOption : MonoBehaviour
{
    public Transform List;
    public bool Open;
    public OptionManager __OptionManager;
    public NextOptionMenu __OptionMenu;

    public void Awake()
    {
        __OptionManager = OptionManager.Get();
    }

    public void Start()
    {
        CreateOption();
    }

    public void Update()
    {
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