using System;
using UnityEngine;

namespace TheIdealShip.Buttons;

public class ButtonBase
{
    public ActionButton ActionButton;
    public GameObject ButtonGameObject;
    public string text;

    public ButtonBase()
    {
        ButtonGameObject = new GameObject(base.ToString());
    }

    public ButtonBase Create(string name = "", GameObject clonetarget = null, Action action = null)
    {
        return null;
    }
}