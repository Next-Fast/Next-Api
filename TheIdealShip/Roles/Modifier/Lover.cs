using UnityEngine;

namespace TheIdealShip.Roles;

public class Lover
{
    public static PlayerControl lover1, lover2;
    public static Color Color = new Color32(255, 105, 180, byte.MaxValue);
    public static bool suicide;

    public static void clearAndReload()
    {
        lover1 = null;
        lover2 = null;
        suicide = false;
    }

    public static void OptionLoad()
    {
        LoverSpawnRate = CustomOption.Create(110, Types.Modifier, cs(Color, "Lover"), rates, null, true);
        LoverIsEvilProbability =
            CustomOption.Create(111, Types.Modifier, "Evil Lover Probability", rates, LoverSpawnRate);
        LoverDieForLove = CustomOption.Create(112, Types.Modifier, "Die For Love", true, LoverSpawnRate);
        LoverPrivateChat = CustomOption.Create(113, Types.Modifier, "Lover Private Chat", false, LoverSpawnRate);
    }
}