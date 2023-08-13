using System;
using AmongUs.GameOptions;

namespace NextShip;

public class VanillaOptionManager
{
    public static IGameOptions VanillaSettings => GameManager.Instance.LogicOptions.currentGameOptions;

    public static string Get(IGameOptions data)
    {
        return Convert.ToBase64String(
            GameOptionsManager.Instance.gameOptionsFactory.ToBytes(GameManager.Instance.LogicOptions
                .currentGameOptions));
    }
}