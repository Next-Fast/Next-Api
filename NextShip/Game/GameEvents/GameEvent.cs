using HarmonyLib;
using NextShip.Listeners;

namespace NextShip.Game.GameEvents;

[HarmonyPatch]
public class GameEvent
{
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd)), HarmonyPostfix]
    public static void AmongUsClient_OnGameEnd(AmongUsClient __instance,
        [HarmonyArgument(0)] EndGameResult endGameResult) =>
        GameEventListener.Start(nameof(IGameEvent.OnGameEnd), __instance, endGameResult);
    
    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start)), HarmonyPostfix]
    public static void GameStartManager_OnGameStart(GameStartManager __instance) =>
        GameEventListener.Start(nameof(IGameEvent.OnGameStart), __instance);
}