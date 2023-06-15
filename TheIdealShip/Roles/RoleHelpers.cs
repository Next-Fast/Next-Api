using System;
using System.Linq;
using System.Collections.Generic;
using TheIdealShip.Roles;
using TheIdealShip.Utilities;
using static TheIdealShip.Languages.Language;
/* using static TheIdealShip.Roles.RoleInfo; */

namespace TheIdealShip
{
    public static class RoleHelpers
    {
        public static readonly RoleId[] AllRoles = EnumHelper.GetAllValues<RoleId>();/* 
        public static RoleInfo GetRoleInfo(PlayerControl player, bool isModifier = false)
        {
            var info = getRoleInfoForPlayer(player, isModifier, false).FirstOrDefault();
            return info;
        }

        public static List<RoleId> GetAllModifierId(this PlayerControl player)
        {
            List<RoleId> allMoidierRoleIds = new List<RoleId>();
            var info = getRoleInfoForPlayer(player, false, true).Where(x => x.type == RoleType.ModifierRole);
            foreach (var i in info)
            {
                allMoidierRoleIds.Add(i.roleId);
            }

            return allMoidierRoleIds;
        } */

        /* public static string GetRoleTeam(PlayerControl p)
        {
            string roleTeam;
            string cre = GetString("Crewmate");
            string imp = GetString("Impostor");
            string neu = GetString("Neutral");
            var info = GetRoleInfo(p);
            switch (info.team)
            {
                case RoleTeam.Crewmate:
                    roleTeam = cre;
                    break;

                case RoleTeam.Impostor:
                    roleTeam = imp;
                    break;

                case RoleTeam.Neutral:
                    roleTeam = neu;
                    break;

                default:
                    roleTeam = "";
                    break;
            }
            return roleTeam;
        } */
        public static bool Is(this PlayerControl player,PlayerControl playerControl)
        {
            return playerControl == player;
        }

/*         public static bool Is(this PlayerControl player, RoleId id)
        {
            var info = GetRoleInfo(player);
            return id == info.roleId;
        } */

/*         public static bool Is(this PlayerControl player, RoleTeam team)
        {
            var info = GetRoleInfo(player);
            return team == info.team;
        } */

/*         public static bool Is(this RoleId id, RoleType type)
        {
            var info = allRoleInfos.Where(x => x.roleId == id).FirstOrDefault();
            return info.type == type;
        } */

        public static bool IsLover(this PlayerControl player)
        {
            return (player.Is(Lover.lover1) || player.Is(Lover.lover2));
        }

        /*         public static bool IsSurvival(this PlayerControl player)
                {
                    return !player.Data.IsDead && !player.Data.Disconnected && player.Data != null && GetRoleInfo(player) != null;
                } */

        public static void Suicide(this PlayerControl player)
        {
            player.MurderPlayer(player);
        }

        static PlayerControl player1;

        public static bool RoleIsH(this PlayerControl player)
        {
            return player != null;
        }

        public static PlayerControl getLover2()
        {
            return CachedPlayer.AllPlayers.Where(x => (x.PlayerControl.IsLover() && x != CachedPlayer.LocalPlayer)).FirstOrDefault();
        }

/*         public static bool RoleIsH(this RoleId id)
        {
            foreach (var p in CachedPlayer.AllPlayers)
            {
                var info = getRoleInfoForPlayer(p,showAll:true);
                foreach (var i in info)
                {
                    if (i.roleId == id) return true;
                }
            }
            return false;
        } */

/*         public static String GetRolesString(PlayerControl p, bool useColors = true, bool isModifier = false)
        {
            string roleName;
            roleName = String.Join("", getRoleInfoForPlayer(p, isModifier).Select(x => useColors ? Helpers.cs(x.color, GetString(x.namekey)) : GetString(x.namekey)).ToArray());
            return roleName;
        } */
    }
}