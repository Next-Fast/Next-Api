using System;
using NextShip.Game.Info;

namespace NextShip.Chat;

public class VoiceClient : IDisposable
{
    public PlayerControl Player;
    public ShipInfo PlayerInfo;
    public VoiceStatus Status;

    public void Dispose()
    {
    }

    public void Start()
    {
    }

    public void Update()
    {
    }
}

public enum VoiceStatus
{
}