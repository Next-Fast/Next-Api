using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TheIdealShip.Utilities;

namespace TheIdealShip.Modules
{
    public class CustomButton
    {
        public static List<CustomButton> buttons = new List<CustomButton>();
        public ActionButton actionButton;
        private Action OnClick;
        public float MaxTimer = float.MaxValue;
        public float Timer = 0f;
        public Func<bool> HasButton;
        public Func<bool> CouldUse;
        public Action OnMeetingEnds;
        public Sprite sprite;
        public Vector3 PositionOffset;
        public HudManager hudManager;
        public KeyCode? hotkey;
        public bool HasEffect;
        public float EffectDuration;
        public Action OnEffectEnds;
        public GameObject actionButtonGameObject;
        public SpriteRenderer actionButtonRenderer;
        public Material actionButtonMat;
        public TextMeshPro actionButtonLabelText;
        public Roles.RoleId roleId;
        public bool isEffectActive = false;
        private static readonly int Desat = Shader.PropertyToID("_Desat");

        public CustomButton
        (
            Action OnClick,
            Action OnMeetingEnds,
            Func<bool> HasButton,
            Func<bool> CouldUse,
            Sprite sprite,
            Vector3 PositionOffset,
            HudManager hudManager,
            KeyCode? hotkey,
            Roles.RoleId roleId,
            bool HasEffect,
            float EffectDuration,
            Action OnEffectEnds
        )
        {
            this.hudManager = hudManager;
            this.OnClick = OnClick;
            this.HasButton = HasButton;
            this.CouldUse = CouldUse;
            this.PositionOffset = PositionOffset;
            this.OnMeetingEnds = OnMeetingEnds;
            this.HasEffect = HasEffect;
            this.EffectDuration = EffectDuration;
            this.OnEffectEnds = OnEffectEnds;
            this.sprite = sprite;
            this.hotkey = hotkey;
            Timer = 16.2f;
            buttons.Add(this);
            actionButton = UnityEngine.Object.Instantiate(hudManager.KillButton, hudManager.KillButton.transform.parent);
            actionButtonGameObject = actionButton.gameObject;
            actionButtonRenderer = actionButton.graphic;
            actionButtonMat = actionButtonRenderer.material;
            actionButtonLabelText = actionButton.buttonLabelText;
            PassiveButton button = actionButton.GetComponent<PassiveButton>();
            button.OnClick = new Button.ButtonClickedEvent();
            button.OnClick.AddListener((UnityEngine.Events.UnityAction)onClickEvent);
            this.roleId = roleId;
        }

        public CustomButton
        (
            Action OnClick,
            Action OnMeetingEnds,
            Func<bool> HasButton,
            Func<bool> CouldUse,
            Sprite sprite,
            Vector3 PositionOffset,
            HudManager hudManager,
            KeyCode? hotkey,
            Roles.RoleId roleId
        )
        : this
        (
            OnClick,
            OnMeetingEnds,
            HasButton,
            CouldUse,
            sprite,
            PositionOffset,
            hudManager,
            hotkey,
            roleId,
            false,
            0f,
            () => { }
        )
        {
        }

/*         public class ButtonPosition
        {
            // 左 left 中 centre 右 right 上 up 下down
            enum PositionBasics
            {
                左,
                中,
                下,
                右,
                左下,
                右下,
            }
            Dictionary<int, Dictionary<PositionBasics, Vector3>> PosDic;
            public Vector3 ButtonP()
            {

            }
        } */

        public void onClickEvent()
        {
            if (this.Timer < 0f && HasButton() && CouldUse())
            {
                actionButtonRenderer.color = new Color(1f,1f,1f,0.3f);
                this.OnClick();


                if (this.HasEffect && !this.isEffectActive)
                {
                    this.Timer = this.EffectDuration;
                    actionButton.cooldownTimerText.color = new Color(0F,0.8F,0F);
                    this.isEffectActive = true;
                }
            }
        }

        public static void HudUpdate()
        {
            buttons.RemoveAll(item => item.actionButton == null);

            for (int i = 0; i < buttons.Count; i++)
            {
                try
                {
                    buttons[i].Update();
                }
                catch (NullReferenceException ex)
                {
                    Exception(ex);
                }
            }
        }

        public static void MeetingEndedUpdate()
        {
            buttons.RemoveAll(item => item.actionButton == null);
            for (int i = 0; i < buttons.Count; i++)
            {
                try
                {
                    buttons[i].OnMeetingEnds();
                    buttons[i].Update();
                }
                catch (NullReferenceException ex)
                {
                    Exception(ex);
                }
            }
        }

        public static void ResetAllCooldowns()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                try
                {
                    buttons[i].Timer = buttons[i].MaxTimer;
                    buttons[i].Update();
                }
                catch (NullReferenceException ex)
                {
                    Exception(ex);
                }
            }
        }
        /*
                public void setActive(bool isActive)
                {
                    if (isActive)
                    {
                        actionButtonGameObject.SetActive(true);
                        actionButtonRenderer.enabled = true;
                    }
                    else
                    {
                        actionButtonGameObject.SetActive(false);
                        actionButtonRenderer.enabled = false;
                    }
                }
        */
        public void setActive(bool isActive)
        {
            if (isActive)
                actionButton.Show();
            else
                actionButton.Hide();
        }

        public void Update()
        {
            var localPlayer = CachedPlayer.LocalPlayer;
            var moveable = localPlayer.PlayerControl.moveable;

            if (!Main.PlayerAndRoleIdDic.ContainsKey((byte)localPlayer.PlayerId)) return;
            if (Main.PlayerAndRoleIdDic[(byte)localPlayer.PlayerId] != roleId || MeetingHud.Instance || ExileController.Instance)
            {
                setActive(false);
                return;
            }

            setActive((hudManager.UseButton.isActiveAndEnabled || hudManager.PetButton.isActiveAndEnabled));

            actionButtonRenderer.sprite = sprite;

            if (HudManager.Instance.UseButton != null)
            {
                Vector3 pos = hudManager.UseButton.transform.localPosition;
                actionButton.transform.localPosition = pos + PositionOffset;
            }
            /*
                        if (CouldUse())
                        {
                            actionButtonRenderer.color = actionButtonLabelText.color = Palette.EnabledColor;
                            actionButtonMat.SetFloat(Desat,0f);
                        }
                        else
                        {
                            actionButtonRenderer.color = actionButtonLabelText.color = Palette.DisabledClear;
                            actionButtonMat.SetFloat(Desat, 1f);
                        }
            */

            string ButtonText = Languages.Language.GetString(roleId.ToString().Replace("RoleId", "") + "ButtonText");
            if (ButtonText != null || ButtonText != "")
            {
                actionButton.OverrideText(ButtonText);
            }
            else
            {
                actionButtonLabelText.enabled = false;
            }

            if (CouldUse())
            {
                actionButton.SetEnabled();
            }
            else
            {
                actionButton.SetDisabled();
            }

            if (Timer >= 0)
            {
                if (HasEffect && isEffectActive)
                    Timer -= Time.deltaTime;
                else if (!localPlayer.PlayerControl.inVent && moveable)
                    Timer -= Time.deltaTime;
            }

            if (Timer <= 0 && HasEffect && isEffectActive)
            {
                isEffectActive = false;
                actionButton.cooldownTimerText.color = Palette.EnabledColor;
                OnEffectEnds();
            }

            actionButton.SetCoolDown(Timer, (HasEffect && isEffectActive) ? EffectDuration : MaxTimer);

            if (hotkey.HasValue && Input.GetKeyDown(hotkey.Value)) onClickEvent();
        }
    }
}