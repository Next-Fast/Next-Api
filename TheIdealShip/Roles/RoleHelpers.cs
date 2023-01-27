using System;
using System.Linq;
using System.Collections.Generic;
using TheIdealShip.Roles;
using static TheIdealShip.Languages.Language;
using static TheIdealShip.Roles.RoleInfo;
using RoleTeam = TheIdealShip.Roles.RoleInfo.RoleTeam;

namespace TheIdealShip
{
    public static class RoleHelpers
    {
        public static RoleInfo GetRoleInfo(PlayerControl player, bool isModifier = false)
        {
            var info = getRoleInfoForPlayer(player, isModifier, false).FirstOrDefault();
            return info;
        }
        public static string GetRoleTeam(PlayerControl p)
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
        }
        public static bool Is(this PlayerControl player,PlayerControl playerControl)
        {
            return playerControl == player;
        }

        public static bool Is(this PlayerControl player, RoleId id)
        {
            var info = GetRoleInfo(player);
            return id == info.roleId;
        }

        public static bool Is(this PlayerControl player, RoleTeam team)
        {
            var info = GetRoleInfo(player);
            return team == info.team;
        }

        public static bool Is(this RoleId id, RoleType type)
        {
            var info = allRoleInfos.Where(x => x.roleId == id).FirstOrDefault();
            return info.type == type;
        }

        public static bool IsSurvival(this PlayerControl player)
        {
            return !player.Data.IsDead && !player.Data.Disconnected && player.Data != null && GetRoleInfo(player) != null;
        }

        public static String GetRolesString(PlayerControl p, bool useColors = true, bool isModifier = false)
        {
            string roleName;
            roleName = String.Join("", getRoleInfoForPlayer(p, isModifier).Select(x => useColors ? Helpers.cs(x.color, GetString(x.namekey)) : GetString(x.namekey)).ToArray());
            return roleName;
        }
    }
}