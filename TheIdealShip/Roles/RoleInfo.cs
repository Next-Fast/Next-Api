using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using UnityEngine;
using static TheIdealShip.Languages.Language;

namespace TheIdealShip.Roles
{
    class RoleInfo
    {
        public Color color;
        public virtual string name { get { return GetString(namekey); } }
        public RoleId roleId;
        string namekey;

        RoleInfo
        (
            string name,
            Color color,
            RoleId roleId
        )
        {
            this.color = color;
            this.namekey = name;
            this.roleId = roleId;
        }

        public static List<RoleInfo> allRoleInfos;

        public static RoleInfo sheriff;
        public static RoleInfo impostor;
        public static RoleInfo crewmate;

        public static void Init()
        {
            sheriff = new RoleInfo("Sheriff",Sheriff.color,RoleId.Sheriff);
            impostor = new RoleInfo("Impostor", Palette.ImpostorRed, RoleId.Impostor);
            crewmate = new RoleInfo("Crewmate", Color.white, RoleId.Crewmate);

            allRoleInfos = new List<RoleInfo>()
            {
                sheriff,
                impostor,
                crewmate
            };
        }

        public static List<RoleInfo> getRoleInfoForPlayer(PlayerControl p, bool showModifier = true)
        {
            List<RoleInfo> infos = new List<RoleInfo>();
            if (p == null) return infos;

            // Modifier
            if (showModifier)
            {
            }

            int count = infos.Count;  // Save count after modifiers are added so that the role count can be checked

            // roles
            if (p == Sheriff.sheriff) infos.Add(sheriff);

            if (infos.Count == count)
            {
                if (p.Data.Role.IsImpostor)
                    infos.Add(impostor);
                else
                    infos.Add(RoleInfo.crewmate);
            }

            return infos;
        }

        public static String GetRolesString(PlayerControl p, bool useColors, bool showModifier = true)
        {
            string roleName;
            roleName = String.Join("",getRoleInfoForPlayer(p,showModifier).Select(x => useColors ? Helpers.cs(x.color, GetString(x.name)) : GetString(x.name)).ToArray());
            return roleName;
        }
    }
}