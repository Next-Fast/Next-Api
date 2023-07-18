namespace NextShip.Game.GameEvents;

public interface IGameEvent
{
    public void OnPlayerJoined(AmongUsClient _amongUsClient) {}
    
    public void OnPlayerLeft(AmongUsClient _amongUsClient) {}

    public void OnGameStart(GameStartManager _gameStartManager) {}
    
    public void OnCoBegin() {}
    
    public void OnPlayerKill(PlayerControl murderer, PlayerControl target) {}
    
    public void OnGameEnd(AmongUsClient __instance, EndGameResult endGameResult) {}
    
    public void OnIntroCutscene(IntroCutscene _introCutscene) {}
    
}