using System;
using System.Reflection;
using UnityEngine;

namespace TheIdealShip.Buttons;

public class ButtonBase
{
    public ActionButton ActionButton;
    public string text;
    public GameObject ButtonGameObject;

    public ButtonBase()
    {
        ButtonGameObject = new GameObject(base.ToString());
    }

    public ButtonBase Create(string name = "", GameObject clonetarget = null, Action action = null)
    {
        return null;
    }
}