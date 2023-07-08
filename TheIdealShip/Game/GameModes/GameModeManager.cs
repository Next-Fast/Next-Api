using TheIdealShip.Options;
using TheIdealShip.Options.OptionValues;
using TheIdealShip.Utilities.Attributes;
using UnityEngine;

namespace TheIdealShip.Game.GameModes;

[Load]
public class GameModeManager
{
    public static OptionBase GameMode;
    public static void CreateOption()
    {
        GameMode = new OptionBase("Game Mode option", 1, optionTab.GameSettings, optionType.String);
        GameMode.StringOptionValue = new StringOptionValue(new[] {"PropHunter", "ResidentEvil", "BattleRoyale"});
    }
}

public enum CustomGameMode
{
    PropHunter = 0, //道具猎手
    ResidentEvil = 1, //生化危机
    BattleRoyale = 2 //大逃杀
}