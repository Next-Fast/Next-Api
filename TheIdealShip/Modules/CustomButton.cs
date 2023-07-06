using System;
using System.Collections.Generic;
using TheIdealShip.Languages;
using TheIdealShip.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace TheIdealShip.Modules;

public class CustomButton
{
    public static List<CustomButton> buttons = new();
    public ActionButton actionButton;
    public GameObject actionButtonGameObject;
    public TextMeshPro actionButtonLabelText;
    public SpriteRenderer actionButtonRenderer;
    public Func<bool> CouldUse;
    public float EffectDuration;
    public Func<bool> HasButton;
    public bool HasEffect;
    public KeyCode? hotkey;
    public HudManager hudManager;
    public bool isEffectActive;
    public float MaxTimer = float.MaxValue;
    private readonly Action OnClick;
    public Action OnEffectEnds;
    public Action OnMeetingEnds;
    public Vector3 PositionOffset;
    public RoleId roleId;
    public Sprite sprite;
    public float Timer;

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
        RoleId roleId,
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
        actionButton = Object.Instantiate(hudManager.KillButton, hudManager.KillButton.transform.parent);
        actionButtonGameObject = actionButton.gameObject;
        actionButtonRenderer = actionButton.graphic;
        actionButtonLabelText = actionButton.buttonLabelText;
        var button = actionButton.GetComponent<PassiveButton>();
        button.OnClick = new Button.ButtonClickedEvent();
        button.OnClick.AddListener((UnityAction)onClickEvent);
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
        RoleId roleId
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
        if (Timer < 0f && HasButton() && CouldUse())
        {
            actionButtonRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            OnClick();


            if (HasEffect && !isEffectActive)
            {
                Timer = EffectDuration;
                actionButton.cooldownTimerText.color = new Color(0F, 0.8F, 0F);
                isEffectActive = true;
            }
        }
    }

    public static void HudUpdate()
    {
        buttons.RemoveAll(item => item.actionButton == null);

        for (var i = 0; i < buttons.Count; i++)
            try
            {
                buttons[i].Update();
            }
            catch (NullReferenceException ex)
            {
                Exception(ex);
            }
    }

    public static void MeetingEndedUpdate()
    {
        buttons.RemoveAll(item => item.actionButton == null);
        for (var i = 0; i < buttons.Count; i++)
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

    public static void ResetAllCooldowns()
    {
        for (var i = 0; i < buttons.Count; i++)
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

        if (!Main.PlayerAndRoleIdDic.ContainsKey(localPlayer.PlayerId)) return;
        if (Main.PlayerAndRoleIdDic[localPlayer.PlayerId] != roleId || MeetingHud.Instance || ExileController.Instance)
        {
            setActive(false);
            return;
        }

        setActive(hudManager.UseButton.isActiveAndEnabled || hudManager.PetButton.isActiveAndEnabled);

        actionButtonRenderer.sprite = sprite;

        if (HudManager.Instance.UseButton != null)
        {
            var pos = hudManager.UseButton.transform.localPosition;
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

        var ButtonText = Language.GetString(roleId.ToString().Replace("RoleId", "") + "ButtonText");
        if (ButtonText == ButtonText.Replace("ButtonText", "") || ButtonText != "")
            actionButton.OverrideText(ButtonText);
        else
            actionButtonLabelText.enabled = false;

        if (CouldUse())
            actionButton.SetEnabled();
        else
            actionButton.SetDisabled();

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

        actionButton.SetCoolDown(Timer, HasEffect && isEffectActive ? EffectDuration : MaxTimer);

        if (hotkey.HasValue && Input.GetKeyDown(hotkey.Value)) onClickEvent();
    }
}