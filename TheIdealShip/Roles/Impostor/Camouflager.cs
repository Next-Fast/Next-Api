using UnityEngine;

namespace TheIdealShip.Roles
{
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
            ButtonSprite = Helpers.LoadSpriteFromResources("TheIdealShip.Resources.CamouflagerButton.png", 115f);
            return ButtonSprite;
        }

        public static void clearAndReload()
        {
            camouflager = null;
            duration = camouflagerDuration.getFloat();
            cooldown = camouflagerCooldown.getFloat();
        }

        public static CustomOption camouflagerSpawnRate;
        public static CustomOption camouflagerCooldown;
        public static CustomOption camouflagerDuration;

        public static void OptionLoad()
        {
            camouflagerSpawnRate = CustomOption.Create(50, Types.Impostor, cs(Camouflager.color, "Camouflager"), rates, null, true);
            camouflagerCooldown = CustomOption.Create(51, Types.Impostor, "Camouflager Cooldown", 30f, 10f, 60f, 2.5f, camouflagerSpawnRate);
            camouflagerDuration = CustomOption.Create(52, Types.Impostor, "Camouflager Duration", 10f, 5f, 20f, 1f, camouflagerSpawnRate);
        }
    }
}