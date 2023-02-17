using System.Collections.Generic;
using static TheIdealShip.Languages.Language;
using UnityEngine;

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
            if (type != RoleType.Modifier)
            {
                switch (type)
                {
                    case RoleType.Crewmate:
                        team = RoleTeam.Crewmate;
                        break;
                    
                    case RoleType.Impostor:
                        team = RoleTeam.Impostor;
                        break;
                    
                    case RoleType.Neutral:
                        team = RoleTeam.Neutral;
                        break;
                }
            }
        }

        public static List<RoleInfo> allRoleInfos;


        // Role 普通职业
        public static RoleInfo impostor;
        public static RoleInfo camouflager;
        public static RoleInfo illusory;

        public static RoleInfo sheriff;
        public static RoleInfo crewmate;

        public static RoleInfo jester;
        public static RoleInfo schrodingersCat;

        // Modifier 附加职业
        public static RoleInfo flash;
        public static RoleInfo lover;


        public static void Init()
        {
            // Role 普通职业
            impostor = new RoleInfo("Impostor", Palette.ImpostorRed, RoleId.Impostor, RoleType.Impostor);
            camouflager = new RoleInfo("Camouflager", Camouflager.color, RoleId.Camouflager, RoleType.Impostor);
            illusory = new RoleInfo("Illusory", Illusory.color, RoleId.Illusory, RoleType.Impostor);

            sheriff = new RoleInfo("Sheriff", Sheriff.color, RoleId.Sheriff, RoleType.Crewmate);
            crewmate = new RoleInfo("Crewmate", Palette.CrewmateBlue, RoleId.Crewmate, RoleType.Crewmate);

            jester = new RoleInfo("Jester", Jester.color, RoleId.Jester, RoleType.Neutral);
            schrodingersCat = new RoleInfo("Schrodinger's Cat", SchrodingersCat.color, RoleId.SchrodingersCats, RoleType.Neutral, SchrodingersCat.team);

            // Modifier 附加职业
            flash = new RoleInfo("Flash", Flash.color, RoleId.Flash, RoleType.Modifier);
            lover = new RoleInfo("Lover", Lover.Color, RoleId.Lover, RoleType.Modifier);

            allRoleInfos = new List<RoleInfo>()
            {
                // Role
                impostor,
                camouflager,
                illusory,

                sheriff,
                crewmate,

                jester,
                schrodingersCat,

                // Modifier
                flash,
                lover
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
                if (p == Lover.lover1 || p == Lover.lover2) infos.Add(lover);
            }

            int count = infos.Count;  // Save count after modifiers are added so that the role count can be checked
            if (!isModifier)
            {
                // roles
                if (p == Sheriff.sheriff) infos.Add(sheriff);
                if (p == Jester.jester) infos.Add(jester);
                if (p == Camouflager.camouflager) infos.Add(camouflager);
                if (p == Illusory.illusory) infos.Add(illusory);
                if (p == SchrodingersCat.schrodingersCat) infos.Add(schrodingersCat);

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