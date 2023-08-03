using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NextShip.Buttons;

public class ButtonBase
{
    public string name;
    public Action Action;
    public ActionButton ActionButton;
    public GameObject ButtonGameObject;
    public string text;

    public ButtonBase()
    {
        ButtonGameObject = new GameObject(base.ToString());
    }

    public ButtonBase Create(string name = "", GameObject clonetarget = null,ActionButton ActionButton = null, Action action = null)
    {
        this.name = name;
        if (clonetarget != null)
        {
            ButtonGameObject = Object.Instantiate(clonetarget);
        }

        if (action != null)
        {
            Action = action;
        }

        if (ActionButton != null)
        {
            this.ActionButton = ActionButton;
        }
        return this;
    }
}