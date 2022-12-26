using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using UnityEngine;

namespace TheIdealShip.Modules
{
    public class CustomOption
    {
        // 定义Type
        public enum CustomOptionType
        {
            General,
            Impostor,
            Neutral,
            Crewmate,
            Modifier,
        }

        public static List<CustomOption> options = new List<CustomOption>();
        public int preset = 0;

        public int id;
        public string name;
        public System.Object[] selections;
        public int defaultSelection;
        public ConfigEntry<int> entry;
        public int selection;
        public OptionBehaviour optionBehaviour;
        public CustomOption parent;
        public bool isHeader;
        public CustomOptionType type;

        // 创建Option
        public CustomOption
        (
            int id,
            CustomOptionType type,
            string name,
            System.Object[] selections,
            System.Object defaultValue,
            CustomOption parent,
            bool isHeader
        )
        {
            this.id = id;
            this.name = parent == null ? name : "- " + name;
            this.selections = selections;
            int index = Array.IndexOf(selections,defaultValue);
            this.defaultSelection = index >= 0 ? index : 0;
            this.parent = parent;
            this.isHeader = isHeader;
            this.type = type;
            selection = 0;
            if (id !=0)
            {
                entry = TheIdealShipPlugin.Instance.Config.Bind($"Preset{preset}", id.ToString(), defaultSelection);
                selection = Mathf.Clamp(entry.Value, 0, selections.Length - 1);
            }
            options.Add(this);
        }

        public static CustomOption Create
        (
            int id,
            CustomOptionType type,
            string name,
            string[] selections,
            CustomOption parent = null,
            bool isHeader =false
        )
        {
            return new CustomOption(id, type, name, selections, "", parent, isHeader);
        }

       // public static
    }
}