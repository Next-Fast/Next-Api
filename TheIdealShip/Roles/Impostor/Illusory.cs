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
            ButtonSprite = SpriteUtils.LoadSpriteFromResources("TheIdealShip.Resources.Illusory.png", 115f);
            return ButtonSprite;
        }

        public static void clearAndReload()
        {
            illusory = null;
            duration = CustomOptionHolder.illusoryDuration.getFloat();
            cooldown = CustomOptionHolder.illusoryCooldown.getFloat();
        }

        public static void OptionLoad()
        {
            illusorySpawnRate = CustomOption.Create(60, Types.Impostor, cs(Illusory.color, "Illusory"), rates, null, true);
            illusoryCooldown = CustomOption.Create(61, Types.Impostor, "Illusory Cooldown", 30f, 10f, 60f, 2.5f, illusorySpawnRate);
            illusoryDuration = CustomOption.Create(62, Types.Impostor, "Illusory Duration", 10f, 5f, 20f, 1f, illusorySpawnRate);
        }
    }
}