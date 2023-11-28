using UnityEngine;

namespace NextShip.Game.Info;

public abstract class ShipInfo
{
    public Color BodyBackGroundColor;
    public Color BodyColor;
    public Color BodyVisorColor;
    public GameData Data;
    public int id;
    public string Name;
    public PlayerControl Player;
}