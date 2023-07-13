using TheIdealShip.Utilities.Attributes;
using UnityEngine;

namespace TheIdealShip.UI.Components;

[Il2CppRegister]
public class DownMenu : MonoBehaviour
{
    public GameObject GameObject;

    public void Start()
    {
        gameObject.AddComponent<DropdownButton>();
    }
}