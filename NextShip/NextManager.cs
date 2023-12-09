using System;
using NextShip.Api.Attributes;
using NextShip.Api.Bases;
using UnityEngine;

namespace NextShip;

[Il2CppRegister]
public class NextManager : MonoBehaviour
{
    private static NextManager _instance;
    
    public static NextManager Instance => _instance ??= Main.Instance.AddComponent<NextManager>();

    public UpdateTasker _Tasker;

    public NextManager()
    {
        _Tasker = new UpdateTasker();
        _instance = this;
    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        _Tasker.FixedUpdate();
    }

    public void LateUpdate()
    {
        _Tasker.LateUpdate();
    }
}