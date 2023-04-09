using System;
using System.Collections.Generic;
using TheIdealShip.Roles;
using TheIdealShip.Utilities;
using static TheIdealShip.Roles.RoleInfo;

namespace TheIdealShip.HistoryManager;

public static class HistoryInfoManager
{
   public static Dictionary<byte, List<HistoryInfo>> HistoryInfoDc = new Dictionary<byte, List<HistoryInfo>>();
   public static Dictionary<byte, int> SerialNumber = new Dictionary<byte, int>();

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

   public static void Add(this PlayerControl player, RoleTeam team, RoleId roleId,bool isRpc, int number = 0)
   {
      if (!HistoryInfoDc.ContainsKey(player.PlayerId) && number != 0) SerialNumber[player.PlayerId] = 0;
      if (number != 0) SerialNumber[player.PlayerId] = number;
      if (!isRpc) RPCHelpers.Create((byte)CustomRPC.HistorySynchronization, new byte[]{ player.PlayerId, (byte)roleId, (byte)team } ,new int[]{ SerialNumber[player.PlayerId] } );
      HistoryInfo info = new HistoryInfo(SerialNumber[player.PlayerId], team, roleId, DateTime.Now);
      HistoryInfoDc[player.PlayerId].Add(info);
      SerialNumber[player.PlayerId]++;
   }
}

public class TeamHistoryManager
{
   public static RoleTeam OldTeam;
   public static RoleTeam NewTeam;

   public static void Start()
   {
      OldTeam = RoleHelpers.GetRoleInfo(CachedPlayer.LocalPlayer.PlayerControl).team;
      NewTeam = OldTeam;
   }

   public static void Update()
   {
      if (NewTeam != OldTeam)
      {
         HistoryInfoManager.Add(CachedPlayer.LocalPlayer.PlayerControl, NewTeam, RoleHistoryManager.NewRole,false);
      }
   }
}

public class RoleHistoryManager
{
   public static RoleId OldRole;
   public static RoleId NewRole;

   public static void Start()
   {
      OldRole = RoleHelpers.GetRoleInfo(CachedPlayer.LocalPlayer.PlayerControl).roleId;
      NewRole = OldRole;
   }

   public static void Update()
   {
      if (NewRole != OldRole)
      {
         HistoryInfoManager.Add(CachedPlayer.LocalPlayer.PlayerControl, TeamHistoryManager.NewTeam, NewRole,false);
      }
   }
}

public class DeathInfoManager
{
   public class DeadPlayer
   {
      public PlayerControl player;
      public DateTime TimeOfDeath;
      public DeathReason DeathReason;
      public PlayerControl Murderer;
      
      public DeadPlayer(PlayerControl player, DateTime TimeOfDeath, DeathReason DeathReason, PlayerControl Murderer)
      {
         this.player = player;
         this.TimeOfDeath = TimeOfDeath;
         this.DeathReason = DeathReason;
         this.Murderer = Murderer;
      }
      
      enum CustomDeathReason 
      {
         
      }
   }
}

