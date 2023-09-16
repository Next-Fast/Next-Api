using System;
using Il2CppInterop.Runtime.Attributes;
using NextShip.Options;
using NextShip.Utilities.Attributes;
using UnityEngine;

namespace NextShip.UI.Components;

[Il2CppRegister]
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
    }
    
    public void CreateOption()
    {
    }

    public void Update()
    {
    }
    

    public void OnDestroy()
    {
        NextOptionMenu.Instance.Initd = false;
        NextOptionMenu.Instance = null;
    }

    public void CreateOptionButton()
    {
        
    }
}