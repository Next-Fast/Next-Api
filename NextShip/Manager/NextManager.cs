using System;
using NextShip.Api.Bases;
using NextShip.Api.UI;
using UnityEngine;

namespace NextShip.Manager;

[Il2CppRegister]
public class NextManager : MonoBehaviour
{
    public readonly UpdateTasker _Tasker = new();

    public void Update()
    {
        NextUIManager.Instance.Update();
        OnUpdate?.Invoke();
    }

    public void FixedUpdate()
    {
        _Tasker.FixedUpdate();
    }

    public void LateUpdate()
    {
        _Tasker.LateUpdate();
    }

    public event Action OnUpdate;
}