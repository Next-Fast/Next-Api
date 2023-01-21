using System;
using UnityEngine;
using TheIdealShip.Roles;

namespace TheIdealShip.Modules
{
    class RoleMenu
    {
        // 做到一半的测试模式职业选择菜单
        public static DialogueBox Dialogue;
        public static bool isCreate = true;
        public static void CreateRoleMenu ()
        {
            var size = new Vector2(10.5f, 5.4f);
            Dialogue = GameObject.Instantiate(HudManager.Instance.Dialogue, HudManager.Instance.transform);
            SpriteRenderer renderer = Dialogue.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            SpriteRenderer closeButton = Dialogue.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
            GameObject fullScreen = renderer.transform.GetChild(0).gameObject;
            fullScreen.GetComponent<PassiveButton>().OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
            fullScreen.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.35f);

            renderer.gameObject.AddComponent<BoxCollider2D>().size = size;
            renderer.color = new Color(1f, 1f, 1f, 0.8f);
            renderer.size = size;

            closeButton.transform.localPosition = new Vector3(-size.x / 2f - 0.3f, size.y / 2f - 0.3f, -10f);
            Dialogue.transform.localScale = new Vector3(1, 1, 1);
            Dialogue.transform.localPosition = new Vector3(0f, 0f, -50f);
            Dialogue.transform.localPosition += new Vector3(0, 0, -500f);

            Dialogue.gameObject.SetActive(false);
            isCreate = false;
            AddButton(new Vector2(0, 0),"1","1",Dialogue.gameObject, Color.white);
        }

        public static void AddButton(Vector2 size, string name, string display,GameObject parent, Color color)
        {
            GameObject obj = new GameObject(name);

            obj.transform.SetParent(parent.transform);

            obj.transform.localScale = new Vector3(1f, 1f, 1f);
            var renderer = obj.AddComponent<SpriteRenderer>();
            var collider = obj.AddComponent<BoxCollider2D>();

            var text = GameObject.Instantiate(HudManager.Instance.Dialogue.target);
            text.transform.SetParent(obj.transform);
            text.transform.localScale = new Vector3(1f, 1f, 1f);
            text.transform.localPosition = new Vector3(0f, 0f, -1f);


            renderer.drawMode = SpriteDrawMode.Tiled;
            renderer.size = size;
            renderer.color = color;

            text.alignment = TMPro.TextAlignmentOptions.Center;
            text.rectTransform.sizeDelta = new Vector2(size.x - 0.15f, 0.2f);
            text.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            text.text = display;
            text.fontSize = text.fontSizeMax = text.fontSizeMax = 2f;
        }
    }
}