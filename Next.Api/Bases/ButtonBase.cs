using Next.Api.Attributes;
using UnityEngine;

namespace Next.Api.Bases;

[Il2CppRegister]
public abstract class ButtonBase : MonoBehaviour
{
    public NKeyBind KeyBind;
    public Action OnClick { get; set; }

    public void Start()
    {
    }

    public void Update()
    {
    }

    public void OnEnable()
    {
    }

    public void OnDisable()
    {
    }

    public void OnDestroy()
    {
    }
}