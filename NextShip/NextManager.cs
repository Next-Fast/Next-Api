using NextShip.Api.Attributes;
using NextShip.Api.Bases;
using NextShip.Api.UI;
using UnityEngine;

namespace NextShip;

[Il2CppRegister]
public class NextManager : MonoBehaviour
{
    private static NextManager _instance;

    public UpdateTasker _Tasker;

    public NextManager()
    {
        _Tasker = new UpdateTasker();
        _instance = this;
    }

    public static NextManager Instance => _instance ??= Main.Instance.AddComponent<NextManager>();

    public void Update()
    {
        NextUIManager.Instance.Update();
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