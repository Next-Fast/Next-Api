using System;
using System.Collections.Generic;
using TheIdealShip.RPC;
using TheIdealShip.Utilities;

/* using static TheIdealShip.Roles.RoleInfo; */

namespace TheIdealShip.HistoryManager;

public static class HistoryInfoManager
{
    public static Dictionary<byte, List<HistoryInfo>> HistoryInfoDc = new();
    public static Dictionary<byte, int> SerialNumber = new();

    public static void Add(this PlayerControl player, RoleTeam team, RoleId roleId, bool isRpc, int number = 0)
    {
        if (!HistoryInfoDc.ContainsKey(player.PlayerId) && number != 0) SerialNumber[player.PlayerId] = 0;
        if (number != 0) SerialNumber[player.PlayerId] = number;
        if (!isRpc)
            RPCHelpers.Create((byte)CustomRPC.HistorySynchronization,
                new[] { player.PlayerId, (byte)roleId, (byte)team }, new[] { SerialNumber[player.PlayerId] });
        var info = new HistoryInfo(SerialNumber[player.PlayerId], team, roleId, DateTime.Now);
        HistoryInfoDc[player.PlayerId].Add(info);
        SerialNumber[player.PlayerId]++;
    }

    public class HistoryInfo
    {
        public static int Number;
        public static RoleTeam Team;
        public static RoleId Role;
        public static DateTime Time;

        public HistoryInfo(int number, RoleTeam team, RoleId role, DateTime time)
        {
            Number = number;
            Team = team;
            Role = role;
            Time = time;
        }
    }
}

public class TeamHistoryManager
{
    public static RoleTeam OldTeam;
    public static RoleTeam NewTeam;

/*    public static void Start()
   {
      OldTeam = RoleHelpers.GetRoleInfo(CachedPlayer.LocalPlayer.PlayerControl).team;
      NewTeam = OldTeam;
   } */

    public static void Update()
    {
        if (NewTeam != OldTeam) CachedPlayer.LocalPlayer.PlayerControl.Add(NewTeam, RoleHistoryManager.NewRole, false);
    }
}

public class RoleHistoryManager
{
    public static RoleId OldRole;
    public static RoleId NewRole;

/*    public static void Start()
   {
      OldRole = RoleHelpers.GetRoleInfo(CachedPlayer.LocalPlayer.PlayerControl).roleId;
      NewRole = OldRole;
   } */

    public static void Update()
    {
        if (NewRole != OldRole) CachedPlayer.LocalPlayer.PlayerControl.Add(TeamHistoryManager.NewTeam, NewRole, false);
    }
}

public class DeathInfoManager
{
    public class DeadPlayer
    {
        public DeathReason DeathReason;
        public PlayerControl Murderer;
        public PlayerControl player;
        public DateTime TimeOfDeath;

        public DeadPlayer(PlayerControl player, DateTime TimeOfDeath, DeathReason DeathReason, PlayerControl Murderer)
        {
            this.player = player;
            this.TimeOfDeath = TimeOfDeath;
            this.DeathReason = DeathReason;
            this.Murderer = Murderer;
        }

        private enum CustomDeathReason
        {
        }
    }
}