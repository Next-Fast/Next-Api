using System;
using NextShip.Options;
using UnityEngine;

namespace NextShip.UI.Components;

public class NextMenuOption : MonoBehaviour
{
    public Transform List;
    public OptionManager __OptionManager;

    public void Awake() => __OptionManager = OptionManager.Get();
    public void Start()
    {
    }

    public void CreateOption()
    {
        
    }

    public void Update()
    {
    }
}