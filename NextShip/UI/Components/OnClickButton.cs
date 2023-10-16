using System;
using NextShip.UI.Interface;
using NextShip.UI.UIManager;
using NextShip.Utilities.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace NextShip.UI.Components;

public class OnClickButton : MonoBehaviour, INextUI
{
    public BoxCollider2D[] BoxCollider2Ds;
    public Button.ButtonClickedEvent OnClick = new();
    public SpriteRenderer ButtonSpriteRenderer;

    public void Start()
    {
        BoxCollider2Ds ??= GetComponents<BoxCollider2D>();

        if (!ButtonSpriteRenderer) ButtonSpriteRenderer = GetComponent<SpriteRenderer>();

        if (BoxCollider2Ds == null && ButtonSpriteRenderer)
        {
            var box = gameObject.AddComponent<BoxCollider2D>();
            box.size = ButtonSpriteRenderer.size;
            BoxCollider2Ds = new[] { box };
        }

        if (!OnClickButtonManager.Get().AllOnClickButtons.Contains(this)) OnClickButtonManager.Instance.Register(this);
    }

    public void OnclickCopy(params Action[] actions)
    {
        OnClick.RemoveAllListeners();
        foreach (var action in actions) OnClick.AddListener(action);
    }
}