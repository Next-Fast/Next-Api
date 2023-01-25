using System.Linq;
using System.Collections.Generic;
using static TheIdealShip.Languages.Language;
using UnityEngine;
using System;

namespace TheIdealShip.Roles
{
    class RoleInfo
    {
        public enum RoleType
        {
            Crewmate,
            Impostor,
            Neutral,
            Modifier,
        }
        public enum RoleTeam
        {
            Crewmate,
            Impostor,
            Neutral
        }
        public RoleType type;
        public RoleTeam team;
        public Color color;
        public virtual string name { get { return GetString(namekey); } }
        public virtual string IntroD { get { return GetString(namekey + "IntroD"); } }
        public virtual string TaskText { get { return GetString(namekey +"TaskText"); } }
        public RoleId roleId;
        public string namekey;

        RoleInfo
        (
            string name,
            Color color,
            RoleId roleId,
            RoleType type,
            RoleTeam team
        )
        {
            this.color = color;
            this.namekey = name;
            this.roleId = roleId;
            this.type = type;
            this.team = team;
        }

        RoleInfo
        (
            string name,
            Color color,
            RoleId roleId,
            RoleType type
        )
        {
            this.color = color;
            this.namekey = name;
            this.roleId = roleId;
            this.type = type;
        }

        public static List<RoleInfo> allRoleInfos;


        // Role 普通职业
        public static RoleInfo sheriff;
        public static RoleInfo impostor;
        public static RoleInfo crewmate;

        // Modifier 附加职业
        public static RoleInfo flash;


        public static void Init()
        {
            // Role 普通职业
            sheriff = new RoleInfo("Sheriff", Sheriff.color, RoleId.Sheriff, RoleType.Crewmate , RoleTeam.Crewmate);
            impostor = new RoleInfo("Impostor", Palette.ImpostorRed, RoleId.Impostor, RoleType.Impostor, RoleTeam.Impostor);
            crewmate = new RoleInfo("Crewmate", Color.white, RoleId.Crewmate, RoleType.Crewmate, RoleTeam.Crewmate);

            // Modifier 附加职业
            flash = new RoleInfo("Flash", Flash.color, RoleId.Flash, RoleType.Modifier);

            allRoleInfos = new List<RoleInfo>()
            {
                // Role
                sheriff,
                impostor,
                crewmate,

                // Modifier
                flash
            };
        }


        public static List<RoleInfo> getRoleInfoForPlayer(PlayerControl p, bool isModifier = false , bool showAll = false)
        {
            List<RoleInfo> infos = new List<RoleInfo>();
            if (p == null) return infos;

            // Modifier
            if (isModifier)
            {
                if (p == Flash.flash) infos.Add(flash);
            }

            int count = infos.Count;  // Save count after modifiers are added so that the role count can be checked
            if (!isModifier)
            {
                // roles
                if (p == Sheriff.sheriff) infos.Add(sheriff);

                if (infos.Count == count)
                {
                    if (p.Data.Role.IsImpostor)
                        infos.Add(impostor);
                    else
                        infos.Add(crewmate);
                }
            }
            return infos;
        }

        public static RoleInfo GetRoleInfo(PlayerControl player, bool isModifier = false)
        {
            var info = getRoleInfoForPlayer(player,isModifier,false).FirstOrDefault();
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
                case RoleTeam.Crewmate :
                    roleTeam = cre;
                    break;

                case RoleTeam.Impostor :
                    roleTeam = imp;
                    break;

                case RoleTeam.Neutral :
                    roleTeam = neu;
                    break;

                default :
                    roleTeam = "";
                    break;
            }
            return roleTeam;
        }

        public static String GetRolesString(PlayerControl p, bool useColors = true, bool isModifier = false)
        {
            string roleName;
            roleName = String.Join("",getRoleInfoForPlayer(p,isModifier).Select(x => useColors ? Helpers.cs(x.color, GetString(x.namekey)) : GetString(x.namekey)).ToArray());
            return roleName;
        }
    }
}