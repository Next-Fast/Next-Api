using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx.Configuration;
using HarmonyLib;
using Hazel;
using NextShip.RPC;
using NextShip.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static NextShip.Languages.Language;
using Object = UnityEngine.Object;

namespace NextShip.Modules;

public class CustomOption
{
    // 定义Type
    public enum CustomOptionType
    {
        General,
        Impostor,
        Neutral,
        Crewmate,
        Modifier
    }

    public static List<CustomOption> options = new();
    public static int preset;
    public Color color;
    public int defaultSelection;
    public ConfigEntry<int> entry;

    public int id;
    public bool isHeader;
    public string name;
    public OptionBehaviour optionBehaviour;
    public CustomOption parent;
    public int selection;
    public object[] selections;
    public CustomOptionType type;

    // 创建Option
    public CustomOption
    (
        int id,
        CustomOptionType type,
        string name,
        object[] selections,
        object defaultValue,
        CustomOption parent,
        bool isHeader
    )
    {
        this.id = id;
        this.name = parent == null ? name : "- " + name;
        this.selections = selections;
        var index = Array.IndexOf(selections, defaultValue);
        defaultSelection = index >= 0 ? index : 0;
        this.parent = parent;
        this.isHeader = isHeader;
        this.type = type;
        selection = 0;
        if (id != 0)
        {
            entry = Main.Instance.Config.Bind($"Preset{preset}", id.ToString(), defaultSelection);
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
        bool isHeader = false
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
        for (var s = min; s <= max; s += step)
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
            new[] { "Off", "On" },
            defaultValue ? "On" : "Off",
            parent,
            isHeader
        );
    }

    public static void switchPreset(int newPreset)
    {
        preset = newPreset;
        foreach (var option in options)
        {
            if (option.id == 0) continue;

            option.entry =
                Main.Instance.Config.Bind($"Preset{preset}", option.id.ToString(),
                    option.defaultSelection);
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
        if (CachedPlayer.AllPlayers.Count <= 1 ||
            (AmongUsClient.Instance!.AmHost == false && CachedPlayer.LocalPlayer.PlayerControl == null)) return;

        var optionsList = new List<CustomOption>(options);
        while (optionsList.Any())
        {
            var amount = (byte)Math.Min(optionsList.Count, 20);
            var writer = AmongUsClient.Instance!.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
                (byte)CustomRPC.ShareOptions, SendOption.Reliable);
            writer.Write(amount);
            for (var i = 0; i < amount; i++)
            {
                var option = optionsList[0];
                optionsList.RemoveAt(0);
                writer.WritePacked((uint)option.id);
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
                else if (entry != null) entry.Value = selection;

                ShareOptionSelections();
            }
        }
    }

    public static string stringOption(string str)
    {
        var s = "";
        if (str.Contains("</color>") && str.Contains("-"))
        {
            s = str.Substring(str.IndexOf("-") + 2);
            s = s.clearColor();
            s = str.Replace(s, GetString(s));
        }
        else if (str.Contains("</color>"))
        {
            s = str.clearColor();
            s = str.Replace(s, GetString(s));
        }
        else if (str.Contains("-"))
        {
            s = str.Substring(str.IndexOf("-") + 2);
            s = str.Replace(s, GetString(s));
        }
        else
        {
            s = GetString(str);
        }

        return s;
    }

    [HarmonyPatch]
    private class GameOptionsMenuPatch
    {
        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
        [HarmonyPostfix]
        public static void SettingsMenu_Postfix(GameOptionsMenu __instance)
        {
            createTabs(__instance);
        }

        private static void createTabs(GameOptionsMenu __instance)
        {
            var isReturn = setNames
            (
                new Dictionary<string, string>
                {
                    ["TISSettings"] = GetString("TISSettings"),
                    ["ImpostorSettings"] = GetString("ImpostorSettings"),
                    ["NeutralSettings"] = GetString("NeutralSettings"),
                    ["CrewmateSettings"] = GetString("CrewmateSettings"),
                    ["ModifierSettings"] = GetString("ModifierSettings")
                }
            );
            if (isReturn) return;

            var template = Object.FindObjectsOfType<StringOption>().FirstOrDefault();
            if (template == null) return;
            var gameSettings = GameObject.Find("Game Settings");
            var gameSettingMenu = Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();

            var tisSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var tisMenu = getMenu(tisSettings, "TISSettings");

            var impostorSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var impostorMenu = getMenu(impostorSettings, "ImpostorSettings");

            var neutralSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var neutralMenu = getMenu(neutralSettings, "NeutralSettings");

            var crewmateSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var crewmateMenu = getMenu(crewmateSettings, "CrewmateSettings");

            var modifierSettings = Object.Instantiate(gameSettings, gameSettings.transform.parent);
            var modifierMenu = getMenu(modifierSettings, "ModifierSettings");

            var roleTab = GameObject.Find("RoleTab");
            var gameTab = GameObject.Find("GameTab");

            var tisTab = Object.Instantiate(roleTab, gameTab.transform.parent);
            var tisTabHighlight = getTabHighlight(tisTab, "NextShipTab", "NextShip.Resources.Tab.png");

            var impostorTab = Object.Instantiate(roleTab, tisTab.transform);
            var impostorTabHighlight = getTabHighlight(impostorTab, "ImpostorTab", "NextShip.Resources.TabI.png");

            var neutralTab = Object.Instantiate(roleTab, impostorTab.transform);
            var neutralTabHighlight = getTabHighlight(neutralTab, "NeutralTab", "NextShip.Resources.TabN.png");

            var crewmateTab = Object.Instantiate(roleTab, neutralTab.transform);
            var crewmateTabHighlight = getTabHighlight(crewmateTab, "CrewmateTab", "NextShip.Resources.TabC.png");

            var modifierTab = Object.Instantiate(roleTab, crewmateTab.transform);
            var modifierTabHighlight = getTabHighlight(modifierTab, "ModifierTab", "NextShip.Resources.TabM.png");

            gameTab.transform.position += Vector3.left * 1.5f;
            tisTab.transform.position += Vector3.left * 1.5f;
            impostorTab.transform.localPosition = Vector3.right * 1f;
            neutralTab.transform.localPosition = Vector3.right * 1f;
            crewmateTab.transform.localPosition = Vector3.right * 1f;
            modifierTab.transform.localPosition = Vector3.right * 1f;

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

            var tabs = new[] { gameTab, tisTab, impostorTab, neutralTab, crewmateTab, modifierTab };
            var settingsHighlightMap = new Dictionary<GameObject, SpriteRenderer>
            {
                [gameSettingMenu.RegularGameSettings] = gameSettingMenu.GameSettingsHightlight,
                [tisSettings.gameObject] = tisTabHighlight,
                [impostorSettings.gameObject] = impostorTabHighlight,
                [neutralSettings.gameObject] = neutralTabHighlight,
                [crewmateSettings.gameObject] = crewmateTabHighlight,
                [modifierSettings.gameObject] = modifierTabHighlight
            };
            for (var i = 0; i < tabs.Length; i++)
            {
                var button = tabs[i].GetComponentInChildren<PassiveButton>();
                if (button == null) continue;
                var copiedIndex = i;
                button.OnClick = new Button.ButtonClickedEvent();
                button.OnClick.AddListener((Action)(() => { setListener(settingsHighlightMap, copiedIndex); }));
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

            var tisOptions = new List<OptionBehaviour>();
            var impostorOptions = new List<OptionBehaviour>();
            var neutralOptions = new List<OptionBehaviour>();
            var crewmateOptions = new List<OptionBehaviour>();
            var modifierOptions = new List<OptionBehaviour>();

            var menus = new List<Transform>
            {
                tisMenu.transform, impostorMenu.transform, neutralMenu.transform, crewmateMenu.transform,
                modifierMenu.transform
            };
            var optionBehaviours = new List<List<OptionBehaviour>>
                { tisOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions };

            for (var i = 0; i < options.Count; i++)
            {
                var option = options[i];
                if ((int)option.type > 4) continue;
                if (option.optionBehaviour == null)
                {
                    var stringOption = Object.Instantiate(template, menus[(int)option.type]);
                    stringOption.OnValueChanged = new Action<OptionBehaviour>(o => { });
                    stringOption.TitleText.text = CustomOption.stringOption(option.name);

                    stringOption.Value = stringOption.oldValue = option.selection;
                    stringOption.ValueText.text =
                        CustomOption.stringOption(option.selections[option.selection].ToString());

                    option.optionBehaviour = stringOption;
                }

                option.optionBehaviour.gameObject.SetActive(true);
            }

            setOptions
            (
                new List<GameOptionsMenu> { tisMenu, impostorMenu, neutralMenu, crewmateMenu, modifierMenu },
                new List<List<OptionBehaviour>>
                    { tisOptions, impostorOptions, neutralOptions, crewmateOptions, modifierOptions },
                new List<GameObject>
                    { tisSettings, impostorSettings, neutralSettings, crewmateSettings, modifierSettings }
            );

            adaptTaskCount(__instance);

            if (roleTab != null)
                roleTab.SetActive(false);
        }

        private static bool setNames(Dictionary<string, string> gameObjectNameDisplayNameMap)
        {
            foreach (var entry in gameObjectNameDisplayNameMap)
                if (GameObject.Find(entry.Key) != null)
                {
                    GameObject.Find(entry.Key).transform.FindChild("GameGroup").FindChild("Text")
                        .GetComponent<TextMeshPro>().SetText(entry.Value);
                    return true;
                }

            return false;
        }

        private static GameOptionsMenu getMenu(GameObject setting, string settingName)
        {
            var menu = setting.transform.FindChild("GameGroup").FindChild("SliderInner")
                .GetComponent<GameOptionsMenu>();
            setting.name = settingName;
            return menu;
        }

        private static SpriteRenderer getTabHighlight(GameObject tab, string tabname, string tabSpritePath)
        {
            var tabHighlight = tab.transform.FindChild("Hat Button").FindChild("Tab Background")
                .GetComponent<SpriteRenderer>();
            tab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite =
                SpriteUtils.LoadSpriteFromResources(tabSpritePath, 100f);
            tab.name = tabname;
            return tabHighlight;
        }

        private static void setListener(Dictionary<GameObject, SpriteRenderer> settingsHighlightMap, int index)
        {
            foreach (var entry in settingsHighlightMap)
            {
                entry.Key.SetActive(false);
                entry.Value.enabled = false;
            }

            settingsHighlightMap.ElementAt(index).Key.SetActive(true);
            settingsHighlightMap.ElementAt(index).Value.enabled = true;
        }

        private static void destroyOptions(List<List<OptionBehaviour>> optionBehavioursList)
        {
            foreach (var optionBehaviours in optionBehavioursList)
            foreach (var option in optionBehaviours)
                Object.Destroy(option.gameObject);
        }

        private static void setOptions(List<GameOptionsMenu> menus, List<List<OptionBehaviour>> options,
            List<GameObject> settings)
        {
            if (!(menus.Count == options.Count && options.Count == settings.Count))
            {
                Error("List counts are not equal");
                return;
            }

            for (var i = 0; i < menus.Count; i++)
            {
                menus[i].Children = options[i].ToArray();
                settings[i].gameObject.SetActive(false);
            }
        }

        private static void adaptTaskCount(GameOptionsMenu __instance)
        {
            var commonTaskOption = __instance.Children.FirstOrDefault(x => x.name == "NumCommonTasks")
                .TryCast<NumberOption>();
            if (commonTaskOption != null) commonTaskOption.ValidRange = new FloatRange(0f, 4f);

            var shortTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumShortTasks")
                .TryCast<NumberOption>();
            if (shortTasksOption != null) shortTasksOption.ValidRange = new FloatRange(0f, 23f);

            var longTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumLongTasks")
                .TryCast<NumberOption>();
            if (longTasksOption != null) longTasksOption.ValidRange = new FloatRange(0f, 15f);
        }
    }

    [HarmonyPatch(typeof(StringOption), nameof(StringOption.OnEnable))]
    public class StringOptionEnablePatch
    {
        public static bool Prefix(StringOption __instance)
        {
            var option = options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;

            __instance.OnValueChanged = new Action<OptionBehaviour>(o => { });
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
            var option = options.FirstOrDefault(option => option.optionBehaviour == __instance);
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
            var option = options.FirstOrDefault(option => option.optionBehaviour == __instance);
            if (option == null) return true;
            option.updateSelection(option.selection - 1);
            return false;
        }
    }

    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
    private class GameOptionsMenuUpdatePatch
    {
        private static float timer = 1f;

        public static void Postfix(GameOptionsMenu __instance)
        {
            // Return Menu Update if in normal among us settings 
            var gameSettingMenu = Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
            if (gameSettingMenu.RegularGameSettings.active || gameSettingMenu.RolesSettings.gameObject.active) return;

            __instance.GetComponentInParent<Scroller>().ContentYBounds.max = -0.5F + __instance.Children.Length * 0.55F;
            timer += Time.deltaTime;
            if (timer < 0.1f) return;
            timer = 0f;

            var offset = 2.75f;
            foreach (var option in options)
            {
                if (GameObject.Find("TISSettings") && option.type != CustomOptionType.General)
                    continue;
                if (GameObject.Find("ImpostorSettings") && option.type != CustomOptionType.Impostor)
                    continue;
                if (GameObject.Find("NeutralSettings") && option.type != CustomOptionType.Neutral)
                    continue;
                if (GameObject.Find("CrewmateSettings") && option.type != CustomOptionType.Crewmate)
                    continue;
                if (GameObject.Find("ModifierSettings") && option.type != CustomOptionType.Modifier)
                    continue;
                if (option?.optionBehaviour != null && option.optionBehaviour.gameObject != null)
                {
                    var enabled = true;
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
                        option.optionBehaviour.transform.localPosition = new Vector3(
                            option.optionBehaviour.transform.localPosition.x, offset,
                            option.optionBehaviour.transform.localPosition.z);
                    }
                }
            }
        }
    }

    [HarmonyPatch]
    private class GameOptionsDataPatch
    {
        private static string buildRoleOptions()
        {
            var impRoles = buildOptionsOfType(CustomOptionType.Impostor, true) + "\n";
            var neutralRoles = buildOptionsOfType(CustomOptionType.Neutral, true) + "\n";
            var crewRoles = buildOptionsOfType(CustomOptionType.Crewmate, true) + "\n";
            var modifiers = buildOptionsOfType(CustomOptionType.Modifier, true);
            return impRoles + neutralRoles + crewRoles + modifiers;
        }

        private static string buildOptionsOfType(CustomOptionType type, bool headerOnly)
        {
            var str = new StringBuilder("\n");
            var options = CustomOption.options.Where(o => o.type == type);

            foreach (var option in options)
                if (option.parent == null)
                {
                    var line = $"{GetString(option.name)}: {option.selections[option.selection]}";
                    str.AppendLine(line);
                }

            if (headerOnly) return str.ToString();
            str = new StringBuilder();

            foreach (var option in options)
            {
                var tName = stringOption(option.name);
                var selStr = GetString(option.selections[option.selection].ToString());

                if (option.parent != null)
                {
                    var isIrrelevant = option.parent.getSelection() == 0 ||
                                       (option.parent.parent != null && option.parent.parent.getSelection() == 0);

                    var c = isIrrelevant ? Color.grey : Color.white; // No use for now
                    if (isIrrelevant) continue;
                    str.AppendLine(TextUtils.cs(c, $"{tName}: {selStr}"));
                }
                else
                {
                    var roleStr = GetString("Roles");
                    if (option == crewmateRolesCountMin)
                    {
                        var optionName = cs(new Color(204f / 255f, 204f / 255f, 0, 1f),
                            GetString("Crewmate") + roleStr);
                        var min = crewmateRolesCountMin.getSelection();
                        var max = crewmateRolesCountMax.getSelection();
                        if (min > max) min = max;
                        var optionValue = min == max ? $"{max}" : $"{min} - {max}";
                        str.AppendLine($"{optionName}: {optionValue}");
                    }
                    else if (option == neutralRolesCountMin)
                    {
                        var optionName = cs(new Color(204f / 255f, 204f / 255f, 0, 1f), GetString("Neutral") + roleStr);
                        var min = neutralRolesCountMin.getSelection();
                        var max = neutralRolesCountMax.getSelection();
                        if (min > max) min = max;
                        var optionValue = min == max ? $"{max}" : $"{min} - {max}";
                        str.AppendLine($"{optionName}: {optionValue}");
                    }
                    else if (option == impostorRolesCountMin)
                    {
                        var optionName = cs(new Color(204f / 255f, 204f / 255f, 0, 1f),
                            GetString("Impostor") + roleStr);
                        var min = impostorRolesCountMin.getSelection();
                        var max = impostorRolesCountMax.getSelection();
                        if (min > max) min = max;
                        var optionValue = min == max ? $"{max}" : $"{min} - {max}";
                        str.AppendLine($"{optionName}: {optionValue}");
                    }
                    else if (option == modifierRolesCountMin)
                    {
                        var optionName = cs(new Color(204f / 255f, 204f / 255f, 0, 1f), GetString("Modifiers"));
                        var min = modifierRolesCountMin.getSelection();
                        var max = modifierRolesCountMax.getSelection();
                        if (min > max) min = max;
                        var optionValue = min == max ? $"{max}" : $"{min} - {max}";
                        str.AppendLine($"{optionName}: {optionValue}");
                    }
                    else if (option == crewmateRolesCountMax || option == neutralRolesCountMax ||
                             option == impostorRolesCountMax || option == modifierRolesCountMax)
                    {
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
            var counter = Main.OptionPage;
            var hudString = "";
            var maxPage = 7;
            switch (counter)
            {
                case 0:
                    hudString += GetString("Page") + "1" + "\n" + GetString("Page1") + __result;
                    break;
                case 1:
                    hudString += GetString("Page") + "2" + "\n" + GetString("Page2") +
                                 buildOptionsOfType(CustomOptionType.General, false);
                    break;
                case 2:
                    hudString += GetString("Page") + "3" + "\n" + GetString("Page3") + buildRoleOptions();
                    break;
                case 3:
                    hudString += GetString("Page") + "4" + "\n" + GetString("Page4") +
                                 buildOptionsOfType(CustomOptionType.Impostor, false);
                    break;
                case 4:
                    hudString += GetString("Page") + "5" + "\n" + GetString("Page5") +
                                 buildOptionsOfType(CustomOptionType.Neutral, false);
                    break;
                case 5:
                    hudString += GetString("Page") + "6" + "\n" + GetString("Page6") +
                                 buildOptionsOfType(CustomOptionType.Crewmate, false);
                    break;
                case 6:
                    hudString += GetString("Page") + "7" + "\n" + GetString("Page7") +
                                 buildOptionsOfType(CustomOptionType.Modifier, false);
                    break;
            }

            hudString += GetString("TABPage") + string.Format("({0}/{1})", counter + 1, maxPage);
            __result = hudString;
        }
    }

    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class GameOptionsNextPagePatch
    {
        public static void Postfix(KeyboardJoystick __instance)
        {
            var optionsPage = Main.OptionPage;
            if (Input.GetKeyDown(KeyCode.Tab)) Main.OptionPage = (Main.OptionPage + 1) % 7;
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                Main.OptionPage = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
                Main.OptionPage = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                Main.OptionPage = 2;
            if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
                Main.OptionPage = 3;
            if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
                Main.OptionPage = 4;
            if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
                Main.OptionPage = 5;
            if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
                Main.OptionPage = 6;
            if (optionsPage != Main.OptionPage)
            {
                var position = (Vector3)FastDestroyableSingleton<HudManager>.Instance?.GameSettings?.transform
                    .localPosition;
                FastDestroyableSingleton<HudManager>.Instance.GameSettings.transform.localPosition =
                    new Vector3(position.x, 2.9f, position.z);
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
            MinX, /*-5.3F*/
            OriginalY = 2.9F,
            MinY = 2.9F;


        public static Scroller Scroller;
        private static Vector3 LastPosition;
        private static float lastAspect;
        private static bool setLastPosition;

        public static void Prefix(HudManager __instance)
        {
            if (__instance.GameSettings?.transform == null) return;

            // Sets the MinX position to the left edge of the screen + 0.1 units
            var safeArea = Screen.safeArea;
            var aspect = Mathf.Min(Camera.main.aspect, safeArea.width / safeArea.height);
            var safeOrthographicSize = CameraSafeArea.GetSafeOrthographicSize(Camera.main);
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
            var LobbyTextRowHeight = 0.06F;
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