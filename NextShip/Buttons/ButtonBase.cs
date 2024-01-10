using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NextShip.Buttons;

public class ButtonBase
{
    public Action Action;
    public ActionButton ActionButton;
    public GameObject ButtonGameObject;
    public string name;
    public string text;

    public ButtonBase()
    {
        ButtonGameObject = new GameObject(base.ToString());
    }

    public ButtonBase Create(string _name = "", GameObject cloneTarget = null, ActionButton _ActionButton = null,
        Action action = null)
    {
        this.name = _name;
        if (cloneTarget != null) ButtonGameObject = Object.Instantiate(cloneTarget);

        if (action != null) Action = action;

        if (_ActionButton != null) this.ActionButton = _ActionButton;
        return this;
    }
}