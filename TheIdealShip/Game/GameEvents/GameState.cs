using HarmonyLib;
using InnerNet;

namespace TheIdealShip.Game.GameEvents;

[HarmonyPatch]
public static class GameState
{
    public static bool InGame = false;
    public static bool IsLobby { get { return AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Joined; } }
    public static bool IsInGame { get { return InGame; } }
    public static bool IsEnded { get { return AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Ended; } }
    public static bool IsOnlineGame { get { return AmongUsClient.Instance.NetworkMode == NetworkModes.OnlineGame; } }
    public static bool IsLocalGame { get { return AmongUsClient.Instance.NetworkMode == NetworkModes.LocalGame; } }
    public static bool IsFreePlay { get { return AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay; } }
    public static bool IsMeeting { get { return InGame && MeetingHud.Instance; } }

    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.CoBegin)), HarmonyPostfix]
    public static void CoBeginPatch() => InGame = true;

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameJoined)), HarmonyPostfix]
    public static void OnGameJoinedPatch() => InGame = false;

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd)), HarmonyPostfix]
    public static void OnGameEndPatch() => InGame = false;
}