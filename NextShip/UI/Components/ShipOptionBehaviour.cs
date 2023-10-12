using NextShip.Options;
using NextShip.UI.Interface;
using NextShip.Utilities.Attributes;
using TMPro;
using UnityEngine;

namespace NextShip.UI.Components;

[Il2CppRegister]
public class ShipOptionBehaviour : MonoBehaviour, INextUI
{
    public SpriteRenderer BackGround_Sprite;
    public TextMeshPro ValueText;
    public TextMeshPro TitleText;
    public PassiveButton PreviousButton;
    public PassiveButton NextButton;
    public OptionBase OptionBase;

    public void FixedUpdate()
    {
    }

    public void OnEnable()
    {
        if (OptionBase == null) return;
        TitleText.text = OptionBase.GetTitleString();
        ValueText.text = OptionBase.GetValueString();
    }
    
    internal static GameObject GenerateOption(Transform Parent, Sprite sprite, string Name = "Temp")
    {
        var Option = new GameObject(Name);
        var behaviour = Option.AddComponent<ShipOptionBehaviour>();
        
        var BackGround = behaviour.BackGround_Sprite = new GameObject("BackGround").AddComponent<SpriteRenderer>();
        BackGround.sprite = sprite;
        
        var ValueText = behaviour.ValueText = new GameObject("ValueText").AddComponent<TextMeshPro>();
        var TitleText = behaviour.TitleText = new GameObject("TitleText").AddComponent<TextMeshPro>();
        
        ValueText.transform.SetParent(Option.transform);
        TitleText.transform.SetParent(Option.transform);
        BackGround.transform.SetParent(Option.transform);

        return Option;
    }
    
}