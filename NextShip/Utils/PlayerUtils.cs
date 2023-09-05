using System.Linq;
using NextShip.Utilities;

namespace NextShip.Utils;

public static class PlayerUtils
{
    public static PlayerControl GetPlayerForId(byte id) => CachedPlayer.AllPlayers.FirstOrDefault(AP => AP.PlayerId == id);
    public static PlayerControl GetPlayerForExile(this GameData.PlayerInfo exile) => GetPlayerForId(exile.PlayerId);

    public static void setDefaultLook(this PlayerControl target)
    {
        target.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId,
            target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
    }

    public static void setLook(this PlayerControl target, string playerName, int colorId, string hatId, string visorId,
        string skinId, string petId)
    {
        target.RawSetColor(colorId);
        target.RawSetHat(hatId, colorId);
        target.RawSetVisor(visorId, colorId);
        target.RawSetName(playerName);
        target.RawSetSkin(skinId, colorId);
        target.RawSetPet(petId, colorId);
    }
}