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
        }
    }
}