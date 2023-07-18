using NextShip.Options;
using NextShip.Utilities.Attributes;

namespace NextShip.Game.GameModes;

[Load]
public class GameModeManager
{
    public static OptionBase GameModeOption;

    public static void CreateOption()
    {
        /*var stringvalue = new StringOptionValue(new[]
        {
            GameMode.Vanilla.ToString(), GameMode.HideAndSeek.ToString(), GameMode.Normal.ToString(),
            GameMode.PropHunter.ToString(),
            GameMode.ResidentEvil.ToString(),
            GameMode.BattleRoyale.ToString(), GameMode.RacingMode.ToString(), GameMode.RunForTime.ToString()
        });
        GameModeOption = CStringOption.Create("Game Mode", stringvalue, optionTab.GameSettings);*/
    }
}

public enum GameMode
{
    Vanilla = -2, // 原版
    HideAndSeek = -1, // 躲猫猫
    Normal = 0, // 一般
    PropHunter = 3, //道具猎手
    ResidentEvil = 4, //生化危机
    BattleRoyale = 5, //大逃杀
    RacingMode = 6, //竞速模式
    RunForTime = 7 //全员加速中
}