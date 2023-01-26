using System.Linq;
using System.Collections.Generic;
using static TheIdealShip.Languages.Language;
using UnityEngine;
using System;

namespace TheIdealShip.Roles
{
    public class RoleInfo
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
        public static RoleInfo impostor;

        public static RoleInfo sheriff;
        public static RoleInfo crewmate;

        public static RoleInfo jester;

        // Modifier 附加职业
        public static RoleInfo flash;


        public static void Init()
        {
            // Role 普通职业
            impostor = new RoleInfo("Impostor", Palette.ImpostorRed, RoleId.Impostor, RoleType.Impostor, RoleTeam.Impostor);

            sheriff = new RoleInfo("Sheriff", Sheriff.color, RoleId.Sheriff, RoleType.Crewmate , RoleTeam.Crewmate);
            crewmate = new RoleInfo("Crewmate", Color.white, RoleId.Crewmate, RoleType.Crewmate, RoleTeam.Crewmate);

            jester = new RoleInfo("Jester", Jester.color, RoleId.Jester, RoleType.Neutral, RoleTeam.Neutral);

            // Modifier 附加职业
            flash = new RoleInfo("Flash", Flash.color, RoleId.Flash, RoleType.Modifier);

            allRoleInfos = new List<RoleInfo>()
            {
                // Role
                impostor,

                sheriff,
                crewmate,

                jester,

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
                if (p == Jester.jester) infos.Add(jester);

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
    }
}