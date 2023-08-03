using NextShip.Utilities.Attributes;
using UnityEngine;

namespace NextShip.UI.Components;

[Il2CppRegister]
public class DownMenu : MonoBehaviour
{
    public GameObject GameObject;

    public void Start()
    {
        gameObject.AddComponent<DropdownButton>();
    }
}