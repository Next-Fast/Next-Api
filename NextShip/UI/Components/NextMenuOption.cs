using System;
using Il2CppInterop.Runtime.Attributes;
using NextShip.Options;
using NextShip.Utilities.Attributes;
using UnityEngine;

namespace NextShip.UI.Components;

public class NextMenuOption : MonoBehaviour
{
    public Transform List;
    public OptionManager __OptionManager;
    public NextOptionMenu __OptionMenu;
    public bool Open = false;

    public void Awake() => __OptionManager = OptionManager.Get();
    
    public void Start()
    {
        CreateOption();
        OpenMenu();
    }
    
    public void OpenMenu()
    {
        if(!ControllerManager.Instance || Open) return;
        ControllerManager.Instance.OpenOverlayMenu(name, __OptionMenu.CloneButton.GetComponent<PassiveButton>());
        Open = true;
    }

    public void CreateOption()
    {
    }

    public void Update()
    {
    }
    
    public void OnDestroy()
    {
        ControllerManager.Instance.CloseOverlayMenu(name);
    }
}