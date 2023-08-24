using System;
using NextShip.UI.UIManager;
using UnityEngine;
using UnityEngine.UI;

namespace NextShip.UI.Components;

public class OnClickButton : MonoBehaviour
{
    public BoxCollider2D[] BoxCollider2Ds;
    public Button.ButtonClickedEvent OnClick = new ();
    public SpriteRenderer ButtonSpriteRenderer;
    
    public void Start()
    {
        if (BoxCollider2Ds == null)
        {
            BoxCollider2Ds = GetComponents<BoxCollider2D>();
        }
        
        if (!ButtonSpriteRenderer)
        {
            ButtonSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (BoxCollider2Ds == null && ButtonSpriteRenderer)
        {
            var box = gameObject.AddComponent<BoxCollider2D>();
            box.size = ButtonSpriteRenderer.size;
            BoxCollider2Ds = new[] { box };
        }

        if (!OnClickButtonManager.AllOnClickButtons.Contains(this))
        {
            this.Register();
        }
    }

    public void OnclickCopy(params Action[] actions)
    {
        OnClick.RemoveAllListeners();
        foreach (var action in actions)
        {
            OnClick.AddListener(action);
        }
    }
}