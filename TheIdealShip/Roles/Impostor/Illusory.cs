using UnityEngine;

namespace TheIdealShip.Roles
{
    public class Illusory
    {
        public static PlayerControl illusory;
        public static Color color = Palette.ImpostorRed;
        public static float cooldown = 30f;
        public static float duration = 10f;
        public static Sprite ButtonSprite;
        public static Sprite getButtonSprite()
        {
            if (ButtonSprite) return ButtonSprite;
            ButtonSprite = Helpers.LoadSpriteFromResources("TheIdealShip.Resources.CamouflagerButton.png", 115f);
            return ButtonSprite;
        }
        
        public static void clearAndReload()
        {
            illusory = null;
            duration = CustomOptionHolder.illusoryDuration.getFloat();
            cooldown = CustomOptionHolder.illusoryCooldown.getFloat();
        }
    }
}