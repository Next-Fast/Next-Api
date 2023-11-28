/*using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.GameOptions;
using HarmonyLib;
using Hazel;
using NextShip.Roles;
using NextShip.RPC;
using NextShip.Utilities;
using UnityEngine;
using static NextShip.Roles.Core.RoleEnum;

namespace NextShip.Patches;

public class RoleAssignmentData
{
    public Dictionary<byte, int> crewSettings = new();
    public Dictionary<byte, int> impSettings = new();
    public Dictionary<byte, int> neutralSettings = new();
    public List<PlayerControl> crewmates { get; set; }
    public List<PlayerControl> impostors { get; set; }
    public int maxCrewmateRoles { get; set; }
    public int maxNeutralRoles { get; set; }
    public int maxImpostorRoles { get; set; }
}

[HarmonyPatch(typeof(RoleOptionsData), nameof(RoleOptionsData.GetNumPerGame))]
internal class RoleOptionsDataGetNumPerGamePatch
{
    public static void Postfix(ref int __result)
    {
    }
}

[HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.GetAdjustedNumImpostors))]
internal class GameOptionsDataGetAdjustedNumImpostorsPatch
{
    public static void Postfix(ref int __result)
    {
        __result = Mathf.Clamp(GameOptionsManager.Instance.CurrentGameOptions.NumImpostors, 1, 3);
    }
}

[HarmonyPatch(typeof(GameOptionsData), nameof(GameOptionsData.Validate))]
internal class GameOptionsDataValidatePatch
{
    public static void Postfix(GameOptionsData __instance)
    {
        __instance.NumImpostors = GameOptionsManager.Instance.CurrentGameOptions.NumImpostors;
    }
}

[HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
internal class RoleManagerSelectRolesPatch
{
    private static int crewValues;
    private static int impValues;
    private static readonly List<Tuple<byte, byte>> playerRoleMap = new();

    public static RoleAssignmentData GetRoleAssignmentData()
    {
        var crewmates = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
        crewmates.RemoveAll(x => x.Data.Role.IsImpostor);
        var impostors = PlayerControl.AllPlayerControls.ToArray().ToList().OrderBy(x => Guid.NewGuid()).ToList();
        impostors.RemoveAll(x => !x.Data.Role.IsImpostor);

        var crewmateMin = crewmateRolesCountMin.getSelection();
        var crewmateMax = crewmateRolesCountMax.getSelection();
        var neutralMin = neutralRolesCountMin.getSelection();
        var neutralMax = neutralRolesCountMax.getSelection();
        var impostorMin = impostorRolesCountMin.getSelection();
        var impostorMax = impostorRolesCountMax.getSelection();

        if (crewmateMin > crewmateMax) crewmateMin = crewmateMax;
        if (neutralMin > neutralMax) neutralMin = neutralMax;
        if (impostorMin > impostorMax) impostorMin = impostorMax;

        var crewCountSettings = rnd.Next(crewmateMin, crewmateMax + 1);
        var neutralCountSettings = rnd.Next(neutralMin, neutralMax + 1);
        var impCountSettings = rnd.Next(impostorMin, impostorMax + 1);

        var maxCrewmateRoles = Mathf.Min(crewmates.Count, crewCountSettings);
        var maxNeutralRoles = Mathf.Min(crewmates.Count, neutralCountSettings);
        var maxImpostorRoles = Mathf.Min(impostors.Count, impCountSettings);

        var impSettings = new Dictionary<byte, int>();
        var neutralSettings = new Dictionary<byte, int>();
        var crewSettings = new Dictionary<byte, int>();

        impSettings.Add((byte)RoleId.Camouflager, Camouflager.camouflagerSpawnRate.getSelection());
        impSettings.Add((byte)RoleId.Illusory, illusorySpawnRate.getSelection());

        crewSettings.Add((byte)RoleId.Sheriff, sheriffSpawnRate.getSelection());

        neutralSettings.Add((byte)RoleId.Jester, jesterSpawnRate.getSelection());
        neutralSettings.Add((byte)RoleId.SchrodingersCats, SchrodingersCatRate.getSelection());

        return new RoleAssignmentData
        {
            crewmates = crewmates,
            impostors = impostors,
            crewSettings = crewSettings,
            neutralSettings = neutralSettings,
            impSettings = impSettings,
            maxCrewmateRoles = maxCrewmateRoles,
            maxNeutralRoles = maxNeutralRoles,
            maxImpostorRoles = maxImpostorRoles
        };
    }

    public static void Postfix()
    {
        var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
            (byte)CustomRPC.ResetVariables, SendOption.Reliable);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        RPCProcedure.ResetVariables();
        if (activateRoles.getBool()) assignRoles();
    }


    private static void assignRoles()
    {
        var data = GetRoleAssignmentData();
        assignEnsuredRoles(data); // Assign roles that should always be in the game next
        assignChanceRoles(data); // Assign roles that may or may not be in the game last
        assignModifiers(); // Assign modifier
        setRolesAgain();
    }

    private static void assignEnsuredRoles(RoleAssignmentData data)
    {
        var ensuredCrewmateRoles = data.crewSettings.Where(x => x.Value == 10).Select(x => x.Key).ToList();
        var ensuredNeutralRoles = data.neutralSettings.Where(x => x.Value == 10).Select(x => x.Key).ToList();
        var ensuredImpostorRoles = data.impSettings.Where(x => x.Value == 10).Select(x => x.Key).ToList();

        while
        (
            (
                data.impostors.Count > 0
                &&
                data.maxImpostorRoles > 0
                &&
                ensuredImpostorRoles.Count > 0
            )
            ||
            (
                data.crewmates.Count > 0
                &&
                (
                    (
                        data.maxCrewmateRoles > 0
                        &&
                        ensuredCrewmateRoles.Count > 0
                    )
                    ||
                    (
                        data.maxNeutralRoles > 0
                        &&
                        ensuredNeutralRoles.Count > 0
                    )
                )
            )
        )
        {
            var rolesToAssign = new Dictionary<RoleType, List<byte>>();
            if
            (
                data.crewmates.Count > 0
                &&
                ensuredCrewmateRoles.Count > 0
            )
                rolesToAssign.Add(RoleType.Crewmate, ensuredCrewmateRoles);

            if
            (
                data.crewmates.Count > 0
                &&
                data.maxNeutralRoles > 0
                &&
                ensuredNeutralRoles.Count > 0
            )
                rolesToAssign.Add(RoleType.Neutral, ensuredNeutralRoles);

            if
            (
                data.impostors.Count > 0
                &&
                data.maxImpostorRoles > 0
                && ensuredImpostorRoles.Count > 0
            )
                rolesToAssign.Add(RoleType.Impostor, ensuredImpostorRoles);

            var roleType = rolesToAssign.Keys.ElementAt(rnd.Next(0, rolesToAssign.Keys.Count()));
            var players = roleType == RoleType.Crewmate || roleType == RoleType.Neutral
                ? data.crewmates
                : data.impostors;
            var index = rnd.Next(0, rolesToAssign[roleType].Count);
            var roleId = rolesToAssign[roleType][index];
            setRoleToRandomPlayer(rolesToAssign[roleType][index], players);
            rolesToAssign[roleType].RemoveAt(index);
            switch (roleType)
            {
                case RoleType.Crewmate:
                    data.maxCrewmateRoles--;
                    crewValues -= 10;
                    break;
                case RoleType.Neutral:
                    data.maxNeutralRoles--;
                    break;
                case RoleType.Impostor:
                    data.maxImpostorRoles--;
                    impValues -= 10;
                    break;
            }
        }
    }

    private static void assignChanceRoles(RoleAssignmentData data)
    {
        // Get all roles where the chance to occur is set grater than 0% but not 100% and build a ticket pool based on their weight
        var crewmateTickets = data.crewSettings.Where(x => x.Value > 0 && x.Value < 10)
            .Select(x => Enumerable.Repeat(x.Key, x.Value)).SelectMany(x => x).ToList();
        var neutralTickets = data.neutralSettings.Where(x => x.Value > 0 && x.Value < 10)
            .Select(x => Enumerable.Repeat(x.Key, x.Value)).SelectMany(x => x).ToList();
        var impostorTickets = data.impSettings.Where(x => x.Value > 0 && x.Value < 10)
            .Select(x => Enumerable.Repeat(x.Key, x.Value)).SelectMany(x => x).ToList();

        // Assign roles until we run out of either players we can assign roles to or run out of roles we can assign to players
        while (
            (data.impostors.Count > 0 && data.maxImpostorRoles > 0 && impostorTickets.Count > 0) ||
            (data.crewmates.Count > 0 && (
                (data.maxCrewmateRoles > 0 && crewmateTickets.Count > 0) ||
                (data.maxNeutralRoles > 0 && neutralTickets.Count > 0)
            )))
        {
            var rolesToAssign = new Dictionary<RoleType, List<byte>>();
            if (data.crewmates.Count > 0 && data.maxCrewmateRoles > 0 && crewmateTickets.Count > 0)
                rolesToAssign.Add(RoleType.Crewmate, crewmateTickets);
            if (data.crewmates.Count > 0 && data.maxNeutralRoles > 0 && neutralTickets.Count > 0)
                rolesToAssign.Add(RoleType.Neutral, neutralTickets);
            if (data.impostors.Count > 0 && data.maxImpostorRoles > 0 && impostorTickets.Count > 0)
                rolesToAssign.Add(RoleType.Impostor, impostorTickets);

            // Randomly select a pool of role tickets to assign a role from next (Crewmate role, Neutral role or Impostor role)
            // then select one of the roles from the selected pool to a player
            // and remove all tickets of this role (and any potentially blocked role pairings) from the pool(s)
            var roleType = rolesToAssign.Keys.ElementAt(rnd.Next(0, rolesToAssign.Keys.Count()));
            var players = roleType == RoleType.Crewmate || roleType == RoleType.Neutral
                ? data.crewmates
                : data.impostors;
            var index = rnd.Next(0, rolesToAssign[roleType].Count);
            var roleId = rolesToAssign[roleType][index];
            setRoleToRandomPlayer(roleId, players);
            rolesToAssign[roleType].RemoveAll(x => x == roleId);

            // Adjust the role limit
            switch (roleType)
            {
                case RoleType.Crewmate:
                    data.maxCrewmateRoles--;
                    break;
                case RoleType.Neutral:
                    data.maxNeutralRoles--;
                    break;
                case RoleType.Impostor:
                    data.maxImpostorRoles--;
                    break;
            }
        }
    }

    private static void assignModifiers()
    {
        var modifierMin = modifierRolesCountMin.getSelection();
        var modifierMax = modifierRolesCountMax.getSelection();
        if (modifierMin > modifierMax) modifierMin = modifierMax;
        var modifierCountSettings = rnd.Next(modifierMin, modifierMax + 1);
        var players = PlayerControl.AllPlayerControls.ToArray().ToList();
        var modifierCount = Mathf.Min(players.Count, modifierCountSettings);
        if (modifierCount == 0) return;

        var allModifiers = new List<RoleId>();
        var ensuredModifiers = new List<RoleId>();
        var chanceModifiers = new List<RoleId>();
        allModifiers.AddRange
        (
            new List<RoleId>
            {
                RoleId.Flash
            }
        );

        if (rnd.Next(1, 101) <= LoverSpawnRate.getSelection() * 10 && CachedPlayer.AllPlayers.Count >= 2)
        {
            // 分配恋人
            var isEvilLover = rnd.Next(1, 101) <= LoverIsEvilProbability.getSelection() * 10;
            byte firstLoverId;
            var impPlayer = new List<PlayerControl>(players);
            var crewPlayer = new List<PlayerControl>(players);
            impPlayer.RemoveAll(x => !x.Data.Role.IsImpostor);
            crewPlayer.RemoveAll(x => x.Data.Role.IsImpostor);

            if (isEvilLover) firstLoverId = setModifierToRandomPlayer((byte)RoleId.Lover, impPlayer);
            else firstLoverId = setModifierToRandomPlayer((byte)RoleId.Lover, crewPlayer);
            var secondLoverId = setModifierToRandomPlayer((byte)RoleId.Lover, crewPlayer, 1);

            players.RemoveAll(x => x.PlayerId == firstLoverId || x.PlayerId == secondLoverId);
            modifierCount--;
        }

        foreach (var m in allModifiers)
            if (getSelectionForRoleId(m) == 10)
                ensuredModifiers.AddRange(Enumerable.Repeat(m, getSelectionForRoleId(m) / 10));
            else chanceModifiers.AddRange(Enumerable.Repeat(m, getSelectionForRoleId(m)));

        assignModifiersToPlayers(ensuredModifiers, players, modifierCount); // Assign ensured modifier

        modifierCount -= ensuredModifiers.Count;
        if (modifierCount <= 0) return;
        var chanceModifierCount = Mathf.Min(modifierCount, chanceModifiers.Count);
        var chanceModifierToAssign = new List<RoleId>();
        while (chanceModifierCount > 0 && chanceModifiers.Count > 0)
        {
            var index = rnd.Next(0, chanceModifiers.Count);
            var modifierId = chanceModifiers[index];
            chanceModifierToAssign.Add(modifierId);

            var modifierSelection = getSelectionForRoleId(modifierId);
            while (modifierSelection > 0)
            {
                chanceModifiers.Remove(modifierId);
                modifierSelection--;
            }

            chanceModifierCount--;
        }
    }

    private static byte setRoleToRandomPlayer(byte roleId, List<PlayerControl> playerList)
    {
        var index = rnd.Next(0, playerList.Count);
        var playerId = playerList[index].PlayerId;

        playerRoleMap.Add(new Tuple<byte, byte>(playerId, roleId));
        var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
            (byte)CustomRPC.SetRole, SendOption.Reliable);
        writer.Write(roleId);
        writer.Write(playerId);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        RPCProcedure.setRole(roleId, playerId);
        return playerId;
    }

    private static void setRolesAgain()
    {
        while (playerRoleMap.Any())
        {
            var amount = (byte)Math.Min(playerRoleMap.Count, 20);
            var writer = AmongUsClient.Instance!.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
                (byte)CustomRPC.WorkaroundSetRoles, SendOption.Reliable);
            writer.Write(amount);
            for (var i = 0; i < amount; i++)
            {
                var option = playerRoleMap[0];
                playerRoleMap.RemoveAt(0);
                writer.WritePacked((uint)option.Item1);
                writer.WritePacked((uint)option.Item2);
            }

            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }

    private static int getSelectionForRoleId(RoleId roleId)
    {
        var selection = 0;
        switch (roleId)
        {
            case RoleId.Flash:
                selection = flashSpawnRate.getSelection();
                break;
        }

        return selection;
    }

    private static byte setModifierToRandomPlayer(byte modifierId, List<PlayerControl> playerList, byte flag = 0)
    {
        if (playerList.Count == 0) return byte.MaxValue;
        var index = rnd.Next(0, playerList.Count);
        var playerId = playerList[index].PlayerId;
        playerList.RemoveAt(index);

        var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
            (byte)CustomRPC.SetModifier, SendOption.Reliable);
        writer.Write(modifierId);
        writer.Write(playerId);
        writer.Write(flag);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        RPCProcedure.setModifier(modifierId, playerId, flag);
        return playerId;
    }

    private static void assignModifiersToPlayers(List<RoleId> modifiers, List<PlayerControl> playerList,
        int modifierCount)
    {
        modifiers = modifiers.OrderBy(x => rnd.Next()).ToList(); // randomize list

        while (modifierCount < modifiers.Count)
        {
            var index = rnd.Next(0, modifiers.Count);
            modifiers.RemoveAt(index);
        }

        byte playerId;

        var crewPlayer = new List<PlayerControl>(playerList);
        /*  crewPlayer.RemoveAll(x => x.Data.Role.IsImpostor || RoleInfo.getRoleInfoForPlayer(x).Any(r => r.type == RoleInfo.RoleType.Neutral)); #1#

        foreach (var modifier in modifiers)
        {
            if (playerList.Count == 0) break;
            playerId = setModifierToRandomPlayer((byte)modifier, playerList);
            playerList.RemoveAll(x => x.PlayerId == playerId);
        }
    }

    private enum RoleType
    {
        Crewmate = 0,
        Neutral = 1,
        Impostor = 2
    }
}*/

