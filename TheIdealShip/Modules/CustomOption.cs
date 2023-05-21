using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using UnityEngine;
using Hazel;
using HarmonyLib;
using System.Text;
using System.Linq;
using TheIdealShip.RPC;
using TheIdealShip.Utilities;
using static TheIdealShip.Languages.Language;
using static TheIdealShip.Helper.Helpers;

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
        public static int preset = 0;

        public int id;
        public string name;
        public Color color;
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

        public static CustomOption Create
        (
            int id,
            CustomOptionType type,
            string name,
            float defaultValue,
            float min,
            float max,
            float step,
            CustomOption parent = null,
            bool isHeader = false
        )
        {
            List<object> selections = new();
            for (float s =min; s <= max; s += step)
                selections.Add(s);

            return new CustomOption
            (
                id,
                type,
                name,
                selections.ToArray(),
                defaultValue,
                parent,
                isHeader
            );
        }

        public static CustomOption Create
        (
            int id,
            CustomOptionType type,
            string name,
            bool defaultValue,
            CustomOption parent = null,
            bool isHeader = false
        )
        {
            return new CustomOption
            (
                id,
                type,
                name,
                new string[]{"Off","On"},
                defaultValue ? "On" : "Off",
                parent,
                isHeader
            );
        }

        public static void switchPreset(int newPreset)
        {
            CustomOption.preset = newPreset;
            foreach (CustomOption option in CustomOption.options)
            {
                if (option.id == 0) continue;

                option.entry = TheIdealShipPlugin.Instance.Config.Bind($"Preset{preset}",option.id.ToString(),option.defaultSelection);
                option.selection = Mathf.Clamp(option.entry.Value, 0, option.selections.Length - 1);
                if (option.optionBehaviour != null && option.optionBehaviour is StringOption stringOption)
                {
                    stringOption.oldValue = stringOption.Value = option.selection;
                    stringOption.ValueText.text = GetString(option.selections[option.selection].ToString());
                }
            }
        }

        public static void ShareOptionSelections()
        {
            if (CachedPlayer.AllPlayers.Count <= 1 || AmongUsClient.Instance!.AmHost == false && CachedPlayer.LocalPlayer.PlayerControl == null) return;

            var optionsList = new List<CustomOption>(CustomOption.options);
            while (optionsList.Any())
            {
                byte amount = (byte) Math.Min(optionsList.Count,20);
                var writer = AmongUsClient.Instance!.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareOptions,SendOption.Reliable,-1);
                writer.Write(amount);
                for (int i = 0; i < amount; i++)
                {
                    var option =optionsList [0];
                    optionsList.RemoveAt(0);
                    writer.WritePacked((uint) option.id);
                    writer.WritePacked(Convert.ToUInt32(option.selection));
                }
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }

        // Getter

        public int getSelection()
        {
            return selection;
        }

        public bool getBool()
        {
            return selection > 0;
        }

        public float getFloat()
        {
            return (float)selections[selection];
        }

        public int getQuantity()
        {
            return selection + 1;
        }

        // Option changes
        public void updateSelection(int newSelection)
        {
            selection = Mathf.Clamp((newSelection + selections.Length) % selections.Length, 0, selections.Length - 1);
            if (optionBehaviour != null && optionBehaviour is StringOption stringOption)
            {
                stringOption.oldValue = stringOption.Value = selection;
                stringOption.ValueText.text = GetString(selections[selection].ToString());

                if (AmongUsClient.Instance?.AmHost == true && CachedPlayer.LocalPlayer.PlayerControl)
                {
                    if (id == 0) switchPreset(selection);
                    else if (entry != null) entry.Value =selection;

                    ShareOptionSelections();
                }
            }
        }

        [HarmonyPatch]
        class GameOptionsMenuPatch
        {
            [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start)), HarmonyPostfix]
            public static void SettingsMenu_Postfix(GameOptionsMenu __instance)
            {
                createTabs(__instance);
            }

            private static void createTabs(GameOptionsMenu __instance)
            {
                bool isReturn = setNames
                (
                    new Dictionary<string, string>()
                    {
                        ["TISSettings"] = GetString("TISSettings"),
                        ["ImpostorSettings"] = GetString("ImpostorSettings"),
                        ["NeutralSettings"] = GetString("NeutralSettings"),
                        ["CrewmateSettings"] = GetString("CrewmateSettings"),
                        ["ModifierSettings"] = GetString("ModifierSettings")
                    }
                );
                if (isReturn) return;

                var template = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();
                if (template == null) return;
                var gameSettings = GameObject.Find("Game Settings");
                var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

                var tisSettings = UnityEngine.Object.Instantiate(gameSettings,gameSettings.transform.parent);
                var tisMenu = getMenu(tisSettings,"TISSettings");

                var impostorSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
                var impostorMenu = getMenu(impostorSettings, "ImpostorSettings");

                var neutralSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
                var neutralMenu = getMenu(neutralSettings, "NeutralSettings");

                var crewmateSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
                var crewmateMenu = getMenu(crewmateSettings, "CrewmateSettings");

                var modifierSettings = UnityEngine.Object.Instantiate(gameSettings, gameSettings.transform.parent);
                var modifierMenu = getMenu(modifierSettings, "ModifierSettings");

                var roleTab = GameObject.Find("RoleTab");
                var gameTab = GameObject.Find("GameTab");

                var tisTab = UnityEngine.Object.Instantiate(roleTab, gameTab.transform.parent);
                var tisTabHighlight = getTabHighlight(tisTab,"TheIdealShipTab","TheIdealShip.Resources.Tab.png");

                var impostorTab = UnityEngine.Object.Instantiate(roleTab, tisTab.transform);
                var impostorTabHighlight = getTabHighlight(impostorTab, "ImpostorTab", "TheIdealShip.Resources.TabI.png");

                var neutralTab = UnityEngine.Object.Instantiate(roleTab, impostorTab.transform);
                var neutralTabHighlight = getTabHighlight(neutralTab, "NeutralTab", "TheIdealShip.Resources.TabN.png");

                var crewmateTab = UnityEngine.Object.Instantiate(roleTab, neutralTab.transform);
                var crewmateTabHighlight = getTabHighlight(crewmateTab, "CrewmateTab", "TheIdealShip.Resources.TabC.png");

                var modifierTab = UnityEngine.Object.Instantiate(roleTab, crewmateTab.transform);
                var modifierTabHighlight = getTabHighlight(modifierTab, "ModifierTab", "TheIdealShip.Resources.TabM.png");

                gameTab.transform.position += Vector3.left * 1.5f;
                tisTab.transform.position += Vector3.left * 1.5f;
                impostorTab.transform.localPosition = Vector3.right * 1f;
                neutralTab.transform.localPosition = Vector3.right* 1f;
                crewmateTab.transform.localPosition = Vector3.right* 1f;
                modifierTab.transform.localPosition = Vector3.right* 1f;

/*                 gameTab.transform.position += Vector3.left * 3.5f;
                tisTab.transform.position += Vector3.left * 4.5f +  Vector3.down * 1f;
                impostorTab.transform.localPosition = Vector3.down * 1f;
                neutralTab.transform.localPosition = Vector3.down * 1f;
                crewmateTab.transform.localPosition = Vector3.down * 1f;
                modifierTab.transform.localPosition = Vector3.down * 1f; */

/*                 gameTab.transform.localScale = Vector3.one;
                tisTab.transform.localScale = Vector3.one;
                impostorTab.transform.localScale = Vector3.one;
                neutralTab.transform.localScale = Vector3.one;
                crewmateTab.transform.localScale = Vector3.one;
                modifierTab.transform.localScale = Vector3.one; */

                var tabs = new GameObject[]{gameTab,tisTab,impostorTab,neutralTab,crewmateTab,modifierTab};
                var settingsHighlightMap = new Dictionary<GameObject,SpriteRenderer>
                {
                    [gameSettingMenu.RegularGameSettings] = gameSettingMenu.GameSettingsHightlight,
                    [tisSettings.gameObject] = tisTabHighlight,
                    [impostorSettings.gameObject] = impostorTabHighlight,
                    [neutralSettings.gameObject] = neutralTabHighlight,
                    [crewmateSettings.gameObject] = crewmateTabHighlight,
                    [modifierSettings.gameObject] = modifierTabHighlight
                };
                for (int i = 0; i < tabs.Length; i++)
                {
                    var button = tabs[i].GetComponentInChildren<PassiveButton>();
                    if (button == null) continue;
                    int copiedIndex = i;
                    button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                    button.OnClick.AddListener((Action)(() => {setListener(settingsHighlightMap, copiedIndex);}));
                }

                destroyOptions(new List<List<OptionBehaviour>>
                {
                    tisMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                    impostorMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                    neutralMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                    crewmateMenu.GetComponentsInChildren<OptionBehaviour>().ToList(),
                    modifierMenu.GetComponentsInChildren<OptionBehaviour>().ToList()
                }
                );

                List<OptionBehaviour> tisOptions = new List<OptionBehaviour>();
                List<OptionBehaviour> impostorOptions = new List<OptionBehaviour>();
                List<OptionBehaviour> neutralOptions = new List<OptionBehaviour>();
                List<OptionBehaviour> crewmateOptions = new List<OptionBehaviour>();
                List<OptionBehaviour> modifierOptions = new List<OptionBehaviour>();

                List<Transform> menus = new List<Transform>() { tisMenu.transform, impostorMenu.transform, neutralMenu.transform, crewmateMenu.transform, modifierMenu.transform };
                List<List<OptionBehaviour>> optionBehaviours = new List<List<OptionBehaviour>>() { tisOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions };

                for (int i = 0; i < CustomOption.options.Count; i++)
                {
                    CustomOption option = CustomOption.options[i];
                    if ((int)option.type > 4) continue;
                    if (option.optionBehaviour == null)
                    {
                        StringOption stringOption = UnityEngine.Object.Instantiate(template,menus[(int)option.type]);
                        stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => {});
                        stringOption.TitleText.text = Helpers.stringOption(option.name);

                        stringOption.Value = stringOption.oldValue = option.selection;
                        stringOption.ValueText.text = Helpers.stringOption(option.selections[option.selection].ToString());

                        option.optionBehaviour = stringOption;
                    }
                    option.optionBehaviour.gameObject.SetActive(true);
                }

                setOptions
                (
                    new List<GameOptionsMenu> { tisMenu, impostorMenu, neutralMenu, crewmateMenu, modifierMenu },
                    new List<List<OptionBehaviour>> { tisOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions },
                    new List<GameObject> { tisSettings, impostorSettings, neutralSettings, crewmateSettings, modifierSettings }
                );

                adaptTaskCount(__instance);

                if (roleTab != null)
                    roleTab.SetActive(false);
            }

            private static bool setNames (Dictionary<string,string> gameObjectNameDisplayNameMap)
            {
                foreach (KeyValuePair <string,string> entry in gameObjectNameDisplayNameMap)
                {
                    if (GameObject.Find(entry.Key) != null)
                    {
                        GameObject.Find(entry.Key).transform.FindChild("GameGroup").FindChild("Text").GetComponent<TMPro.TextMeshPro>().SetText(entry.Value);
                        return true;
                    }
                }
                return false;
            }

            private static GameOptionsMenu getMenu(GameObject setting, string settingName)
            {
                var menu = setting.transform.FindChild("GameGroup").FindChild("SliderInner").GetComponent<GameOptionsMenu>();
                setting.name = settingName;
                return menu;
            }

            private static SpriteRenderer getTabHighlight (GameObject tab,string tabname,string tabSpritePath)
            {
                var tabHighlight = tab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
                tab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Helpers.LoadSpriteFromResources(tabSpritePath,100f);
                tab.name = tabname;
                return tabHighlight;
            }

            private static void setListener(Dictionary<GameObject,SpriteRenderer> settingsHighlightMap,int index)
            {
                foreach (KeyValuePair<GameObject,SpriteRenderer> entry in settingsHighlightMap)
                {
                    entry.Key.SetActive(false);
                    entry.Value.enabled = false;
                }
                settingsHighlightMap.ElementAt(index).Key.SetActive(true);
                settingsHighlightMap.ElementAt(index).Value.enabled = true;
            }

            private static void destroyOptions (List<List<OptionBehaviour>> optionBehavioursList)
            {
                foreach (List<OptionBehaviour> optionBehaviours in optionBehavioursList)
                {
                    foreach (OptionBehaviour option in optionBehaviours)
                    {
                        UnityEngine.Object.Destroy(option.gameObject);
                    }
                }
            }

            private static void setOptions (List<GameOptionsMenu> menus,List<List<OptionBehaviour>> options,List<GameObject> settings)
            {
                if (!(menus.Count == options.Count && options.Count == settings.Count))
                {
                    TheIdealShipPlugin.Logger.LogError("List counts are not equal");
                    return;
                }
                for (int i = 0; i < menus.Count; i++)
                {
                    menus[i].Children = options[i].ToArray();
                    settings[i].gameObject.SetActive(false);
                }
            }

            private static void adaptTaskCount(GameOptionsMenu __instance)
            {
                var commonTaskOption = __instance.Children.FirstOrDefault(x => x.name == "NumCommonTasks").TryCast<NumberOption>();
                if (commonTaskOption != null) commonTaskOption.ValidRange = new FloatRange(0f,4f);

                var shortTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumShortTasks").TryCast<NumberOption>();
                if (shortTasksOption != null) shortTasksOption.ValidRange = new FloatRange(0f, 23f);

                var longTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumLongTasks").TryCast<NumberOption>();
                if (longTasksOption != null) longTasksOption.ValidRange = new FloatRange(0f, 15f);
            }
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
        public class StringOptionEnablePatch
        {
            public static bool Prefix(StringOption __instance)
            {
                CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
                if (option == null) return true;

                __instance.OnValueChanged = new Action<OptionBehaviour>((o) => { });
                __instance.TitleText.text = stringOption(option.name);
                __instance.Value = __instance.oldValue = option.selection;
                __instance.ValueText.text = GetString(option.selections[option.selection].ToString());

                return false;
            }
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.Increase))]
        public class StringOptionIncreasePatch
        {
            public static bool Prefix(StringOption __instance)
            {
                CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
                if (option == null) return true;
                option.updateSelection(option.selection + 1);
                return false;
            }
        }

        [HarmonyPatch(typeof(StringOption), nameof(StringOption.Decrease))]
        public class StringOptionDecreasePatch
        {
            public static bool Prefix(StringOption __instance)
            {
                CustomOption option = CustomOption.options.FirstOrDefault(option => option.optionBehaviour == __instance);
                if (option == null) return true;
                option.updateSelection(option.selection - 1);
                return false;
            }
        }

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        class GameOptionsMenuUpdatePatch
        {
            private static float timer = 1f;
            public static void Postfix(GameOptionsMenu __instance)
            {
                // Return Menu Update if in normal among us settings 
                var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
                if (gameSettingMenu.RegularGameSettings.active || gameSettingMenu.RolesSettings.gameObject.active) return;

                __instance.GetComponentInParent<Scroller>().ContentYBounds.max = -0.5F + __instance.Children.Length * 0.55F;
                timer += Time.deltaTime;
                if (timer < 0.1f) return;
                timer = 0f;

                float offset = 2.75f;
                foreach (CustomOption option in CustomOption.options)
                {
                    if (GameObject.Find("TISSettings") && option.type != CustomOption.CustomOptionType.General)
                        continue;
                    if (GameObject.Find("ImpostorSettings") && option.type != CustomOption.CustomOptionType.Impostor)
                        continue;
                    if (GameObject.Find("NeutralSettings") && option.type != CustomOption.CustomOptionType.Neutral)
                        continue;
                    if (GameObject.Find("CrewmateSettings") && option.type != CustomOption.CustomOptionType.Crewmate)
                        continue;
                    if (GameObject.Find("ModifierSettings") && option.type != CustomOption.CustomOptionType.Modifier)
                        continue;
                    if (option?.optionBehaviour != null && option.optionBehaviour.gameObject != null)
                    {
                        bool enabled = true;
                        var parent = option.parent;
                        while (parent != null && enabled)
                        {
                            enabled = parent.selection != 0;
                            parent = parent.parent;
                        }
                        option.optionBehaviour.gameObject.SetActive(enabled);
                        if (enabled)
                        {
                            offset -= option.isHeader ? 0.75f : 0.5f;
                            option.optionBehaviour.transform.localPosition = new Vector3(option.optionBehaviour.transform.localPosition.x, offset, option.optionBehaviour.transform.localPosition.z);
                        }
                    }
                }
            }
        }

        [HarmonyPatch]
        class GameOptionsDataPatch
        {
            private static string buildRoleOptions()
            {
                var impRoles = buildOptionsOfType(CustomOption.CustomOptionType.Impostor, true) + "\n";
                var neutralRoles = buildOptionsOfType(CustomOption.CustomOptionType.Neutral, true) + "\n";
                var crewRoles = buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, true) + "\n";
                var modifiers = buildOptionsOfType(CustomOption.CustomOptionType.Modifier, true);
                return impRoles + neutralRoles + crewRoles + modifiers;
            }

            private static string buildOptionsOfType(CustomOption.CustomOptionType type, bool headerOnly)
            {
                StringBuilder str = new StringBuilder("\n");
                var options = CustomOption.options.Where(o => o.type == type);

                foreach (var option in options)
                {
                    if (option.parent == null)
                    {
                        string line = $"{GetString(option.name)}: {option.selections[option.selection].ToString()}";
                        str.AppendLine(line);
                    }
                }
                if (headerOnly) return str.ToString();
                else str = new StringBuilder();

                foreach (CustomOption option in options)
                {
                    string tName = stringOption(option.name);
                    string selStr = GetString(option.selections[option.selection].ToString());

                    if (option.parent != null)
                    {
                        bool isIrrelevant = option.parent.getSelection() == 0 || (option.parent.parent != null && option.parent.parent.getSelection() == 0);

                        Color c = isIrrelevant ? Color.grey : Color.white;  // No use for now
                        if (isIrrelevant) continue;
                        str.AppendLine(Helpers.cs(c, $"{tName}: {selStr}"));
                    }
                    else
                    {
                        var roleStr = GetString("Roles");
                        if (option == CustomOptionHolder.crewmateRolesCountMin)
                        {
                            var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), GetString("Crewmate") + roleStr);
                            var min = CustomOptionHolder.crewmateRolesCountMin.getSelection();
                            var max = CustomOptionHolder.crewmateRolesCountMax.getSelection();
                            if (min > max) min = max;
                            var optionValue = (min == max) ? $"{max}" : $"{min} - {max}";
                            str.AppendLine($"{optionName}: {optionValue}");
                        }
                        else if (option == CustomOptionHolder.neutralRolesCountMin)
                        {
                            var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), GetString("Neutral") + roleStr);
                            var min = CustomOptionHolder.neutralRolesCountMin.getSelection();
                            var max = CustomOptionHolder.neutralRolesCountMax.getSelection();
                            if (min > max) min = max;
                            var optionValue = (min == max) ? $"{max}" : $"{min} - {max}";
                            str.AppendLine($"{optionName}: {optionValue}");
                        }
                        else if (option == CustomOptionHolder.impostorRolesCountMin)
                        {
                            var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), GetString("Impostor") + roleStr);
                            var min = CustomOptionHolder.impostorRolesCountMin.getSelection();
                            var max = CustomOptionHolder.impostorRolesCountMax.getSelection();
                            if (min > max) min = max;
                            var optionValue = (min == max) ? $"{max}" : $"{min} - {max}";
                            str.AppendLine($"{optionName}: {optionValue}");
                        }
                        else if (option == CustomOptionHolder.modifierRolesCountMin)
                        {
                            var optionName = CustomOptionHolder.cs(new Color(204f / 255f, 204f / 255f, 0, 1f), GetString("Modifiers"));
                            var min = CustomOptionHolder.modifierRolesCountMin.getSelection();
                            var max = CustomOptionHolder.modifierRolesCountMax.getSelection();
                            if (min > max) min = max;
                            var optionValue = (min == max) ? $"{max}" : $"{min} - {max}";
                            str.AppendLine($"{optionName}: {optionValue}");
                        }
                        else if ((option == CustomOptionHolder.crewmateRolesCountMax) || (option == CustomOptionHolder.neutralRolesCountMax) || (option == CustomOptionHolder.impostorRolesCountMax) || option == CustomOptionHolder.modifierRolesCountMax)
                        {
                            continue;
                        }
                        else
                        {
                            str.AppendLine($"\n{tName}: {selStr}");
                        }
                    }
                }
                return str.ToString();
            }

            [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.ToHudString))]
            private static void Postfix(ref string __result)
            {
                var counter = TheIdealShipPlugin.OptionPage;
                string hudString = "";
                int maxPage = 7;
                switch (counter)
                {
                    case 0:
                        hudString += GetString("Page") + "1" + "\n" + GetString("Page1") + __result;
                        break;
                    case 1:
                        hudString += GetString("Page") + "2" + "\n" + GetString("Page2") + buildOptionsOfType(CustomOption.CustomOptionType.General, false);
                        break;
                    case 2:
                        hudString += GetString("Page") + "3" + "\n" + GetString("Page3") + buildRoleOptions();
                        break;
                    case 3:
                        hudString += GetString("Page") + "4" + "\n" + GetString("Page4") + buildOptionsOfType(CustomOption.CustomOptionType.Impostor, false);
                        break;
                    case 4:
                        hudString += GetString("Page") + "5" + "\n" + GetString("Page5") + buildOptionsOfType(CustomOption.CustomOptionType.Neutral, false);
                        break;
                    case 5:
                        hudString += GetString("Page") + "6" + "\n" + GetString("Page6") + buildOptionsOfType(CustomOption.CustomOptionType.Crewmate, false);
                        break;
                    case 6:
                        hudString += GetString("Page") + "7" + "\n" +  GetString("Page7") + buildOptionsOfType(CustomOption.CustomOptionType.Modifier, false);
                        break;
                }

                hudString += GetString("TABPage") + string.Format("({0}/{1})",counter + 1, maxPage);
                __result = hudString;
            }
        }

        [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
        public static class GameOptionsNextPagePatch
        {
            public static void Postfix(KeyboardJoystick __instance)
            {
                int optionsPage = TheIdealShipPlugin.OptionPage;
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    TheIdealShipPlugin.OptionPage = (TheIdealShipPlugin.OptionPage + 1) % 7;
                }
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    TheIdealShipPlugin.OptionPage = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                {
                    TheIdealShipPlugin.OptionPage = 1;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    TheIdealShipPlugin.OptionPage = 2;
                }
                if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                {
                    TheIdealShipPlugin.OptionPage = 3;
                }
                if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
                {
                    TheIdealShipPlugin.OptionPage = 4;
                }
                if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
                {
                    TheIdealShipPlugin.OptionPage = 5;
                }
                if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
                {
                    TheIdealShipPlugin.OptionPage = 6;
                }
                if (optionsPage != TheIdealShipPlugin.OptionPage)
                {
                    Vector3 position = (Vector3)FastDestroyableSingleton<HudManager>.Instance?.GameSettings?.transform.localPosition;
                    FastDestroyableSingleton<HudManager>.Instance.GameSettings.transform.localPosition = new Vector3(position.x, 2.9f, position.z);
                }
            }
        }


        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public class GameSettingsScalePatch
        {
            public static void Prefix(HudManager __instance)
            {
                if (__instance.GameSettings != null) __instance.GameSettings.fontSize = 1.2f;
            }
        }


      // This class is taken from Town of Us Reactivated, https://github.com/eDonnes124/Town-Of-Us-R/blob/master/source/Patches/CustomOption/Patches.cs, Licensed under GPLv3
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public class HudManagerUpdate
        {
            public static float
                MinX,/*-5.3F*/
                OriginalY = 2.9F,
                MinY = 2.9F;


            public static Scroller Scroller;
            private static Vector3 LastPosition;
            private static float lastAspect;
            private static bool setLastPosition = false;

            public static void Prefix(HudManager __instance)
            {
                if (__instance.GameSettings?.transform == null) return;

                // Sets the MinX position to the left edge of the screen + 0.1 units
                Rect safeArea = Screen.safeArea;
                float aspect = Mathf.Min((Camera.main).aspect, safeArea.width / safeArea.height);
                float safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(Camera.main);
                MinX = 0.1f - safeOrthographicSize * aspect;

                if (!setLastPosition || aspect != lastAspect)
                {
                    LastPosition = new Vector3(MinX, MinY);
                    lastAspect = aspect;
                    setLastPosition = true;
                    if (Scroller != null) Scroller.ContentXBounds = new FloatRange(MinX, MinX);
                }

                CreateScroller(__instance);

                Scroller.gameObject.SetActive(__instance.GameSettings.gameObject.activeSelf);

                if (!Scroller.gameObject.active) return;

                var rows = __instance.GameSettings.text.Count(c => c == '\n');
                float LobbyTextRowHeight = 0.06F;
                var maxY = Mathf.Max(MinY, rows * LobbyTextRowHeight + (rows - 38) * LobbyTextRowHeight);

                Scroller.ContentYBounds = new FloatRange(MinY, maxY);

                // Prevent scrolling when the player is interacting with a menu
                if (CachedPlayer.LocalPlayer?.PlayerControl.CanMove != true)
                {
                    __instance.GameSettings.transform.localPosition = LastPosition;

                    return;
                }

                if (__instance.GameSettings.transform.localPosition.x != MinX ||
                    __instance.GameSettings.transform.localPosition.y < MinY) return;

                LastPosition = __instance.GameSettings.transform.localPosition;
            }

            private static void CreateScroller(HudManager __instance)
            {
                if (Scroller != null) return;

                Scroller = new GameObject("SettingsScroller").AddComponent<Scroller>();
                Scroller.transform.SetParent(__instance.GameSettings.transform.parent);
                Scroller.gameObject.layer = 5;

                Scroller.transform.localScale = Vector3.one;
                Scroller.allowX = false;
                Scroller.allowY = true;
                Scroller.active = true;
                Scroller.velocity = new Vector2(0, 0);
                Scroller.ScrollbarYBounds = new FloatRange(0, 0);
                Scroller.ContentXBounds = new FloatRange(MinX, MinX);
                Scroller.enabled = true;

                Scroller.Inner = __instance.GameSettings.transform;
                __instance.GameSettings.transform.SetParent(Scroller.transform);
            }
        }
    }
}