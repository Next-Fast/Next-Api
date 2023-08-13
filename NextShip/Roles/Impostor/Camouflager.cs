using UnityEngine;

namespace NextShip.Roles;

public class Camouflager
{
    public static PlayerControl camouflager;
    public static Color color = Palette.ImpostorRed;
    public static float cooldown = 30f;
    public static float duration = 10f;
    public static Sprite ButtonSprite;
    

    public static Sprite getButtonSprite()
    {
        if (ButtonSprite) return ButtonSprite;
        ButtonSprite = SpriteUtils.LoadSpriteFromResources("NextShip.Resources.CamouflagerButton.png", 115f);
        return ButtonSprite;
    }

    public static void clearAndReload()
    {
        camouflager = null;
    }

    public static void OptionLoad()
    {
    }
}