using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace NextShip.UI;

[HarmonyPatch]
public class VoiceChatHud
{
    private static readonly List<GameObject> AllPlayer = new();
    private static GameObject VCHud;
    private static GameObject VCClinetTem;
    private static Vector3 POS;
    private static GameObject AllPlayerList;

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    [HarmonyPostfix]
    public static void HudManager_Start_Postfix(HudManager __instance)
    {
        return;
        if (VCHud) return;
        VCHud = new GameObject("VoiceChatHud")
        {
            transform =
            {
                localPosition = new Vector3(5, 0, -500)
            }
        };
        VCHud.transform.SetParent(Camera.main!.transform);
        var HeadBackGround = new GameObject("HeadBackGround");
        HeadBackGround.transform.SetParent(VCHud.transform);
        var BackGroundSprite = HeadBackGround.AddComponent<SpriteRenderer>();
        BackGroundSprite.sprite = SpriteUtils.LoadSpriteFromResources("NextShip.Resources.HeadBackGround.png", 150f);
        BackGroundSprite.size = new Vector2(0.8f, 12);
        BackGroundSprite.drawMode = SpriteDrawMode.Sliced;
        BackGroundSprite.color = new Color(1, 1, 1, 0.399f);
        BackGroundSprite.transform.localPosition = new Vector3(0, 0, 0);

        AllPlayerList = new GameObject("AllPlayer");
        AllPlayerList.transform.SetParent(VCHud.transform);
        AllPlayerList.transform.localPosition = new Vector3(0, 0, -5);
        POS = new Vector3(0, 0, -5);

        VCHud.AllGameObjectDo(n => n.layer = LayerUtils.GetUILayer());

        var chatButton = GameObject.Find("ChatButton");
        chatButton.transform.localPosition = new Vector3(3.7f, 2.6f, -100);
        var Buttons = GameObject.Find("Buttons");
        Buttons.transform.localPosition = new Vector3(-0.5f, 0, 0);
        Buttons.transform.Find("BottomRight").localPosition = new Vector3(4.85f, -2.3f, -9);
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    [HarmonyPostfix]
    public static void HudManagerUpdatePatch_Postfix(HudManager __instance)
    {
        if (!VCClinetTem) return;


        UpdatePlayer(__instance);
    }

    private static void UpdatePlayer(HudManager __instance)
    {
        foreach (var avatar in CachedPlayer.AllPlayers.Select(_ => new GameObject("Avatar")))
        {
            avatar.transform.SetParent(AllPlayerList.transform);
            /*avatar.transform.localPosition = Pos;*/
            var Border = new GameObject("Border");
            Border.transform.SetParent(avatar.transform);
            Border.transform.localPosition = new Vector3(0, 0, 0);
            var BorderSprite = Border.AddComponent<SpriteRenderer>();
            BorderSprite.sprite = SpriteUtils.LoadSpriteFromResources("NextShip.Resources.HeadBorder.png", 500f);

            AllPlayer.Add(avatar);
            /*Pos.y -= 2;*/
        }
    }
}