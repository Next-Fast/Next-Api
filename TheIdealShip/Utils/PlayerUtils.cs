using TheIdealShip.Utilities;
using UnityEngine;

namespace TheIdealShip.Utils;

public static class PlayerUtils
{
    public static PlayerControl GetPlayerForId(byte id)
    {
        foreach (var AP in CachedPlayer.AllPlayers)
        {
            if (AP.PlayerId == id)
            {
                return AP;
            }
        }
        return null;
    }

    public static PlayerControl GetPlayerForExile(this GameData.PlayerInfo exile)
    {
        var p = GetPlayerForId(exile.PlayerId);
        return p;
    }

    public static void setDefaultLook(this PlayerControl target) => target.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);

    public static void setLook(this PlayerControl target, string playerName, int colorId, string hatId, string visorId, string skinId, string petId)
    {
        target.RawSetColor(colorId);
        target.RawSetHat(hatId, colorId);
        target.RawSetVisor(visorId, colorId);
        target.RawSetName(playerName);

        SkinViewData nextSkin = skinId.GetSkinViewDataById();

        PlayerPhysics playerPhysics = target.MyPhysics;
        AnimationClip clip = null;
        var spriteAnim = playerPhysics.myPlayer.cosmetics.skin.animator;
        var currentPhysicsAnim = playerPhysics.Animations.Animator.GetCurrentAnimation();

        var group = playerPhysics.Animations.group;
        if (currentPhysicsAnim == group.RunAnim) clip = nextSkin.RunAnim;
        if (currentPhysicsAnim == group.SpawnAnim) clip = nextSkin.SpawnAnim;
        if (currentPhysicsAnim == group.EnterVentAnim) clip = nextSkin.EnterVentAnim;
        if (currentPhysicsAnim == group.ExitVentAnim) clip = nextSkin.ExitVentAnim;
        if (currentPhysicsAnim == group.IdleAnim) clip = nextSkin.IdleAnim;

        float progress = playerPhysics.Animations.Animator.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        playerPhysics.myPlayer.cosmetics.skin.skin = nextSkin;
        playerPhysics.myPlayer.cosmetics.skin.UpdateMaterial();

        spriteAnim.Play(clip, 1f);
        spriteAnim.m_animator.Play("a", 0, progress % 1);
        spriteAnim.m_animator.Update(0f);

        if (target.cosmetics.currentPet) UnityEngine.Object.Destroy(target.cosmetics.currentPet.gameObject);
        target.cosmetics.currentPet = UnityEngine.Object.Instantiate<PetBehaviour>(petId.GetPetBehaviourById());
        target.cosmetics.currentPet.transform.position = target.transform.position;
        target.cosmetics.currentPet.Source = target;
        target.cosmetics.currentPet.Visible = target.Visible;
        target.SetPlayerMaterialColors(target.cosmetics.currentPet.rend);
    }
}