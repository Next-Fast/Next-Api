using System.Collections.Generic;
using HarmonyLib;

namespace NextShip.UI.UIManager;

[HarmonyPatch]
public class ControlManager
{
    public static ControlManager Instance;

    internal List<ShipAction> allShipActions = new ();

    static ControlManager()
    {
        Instance = new ControlManager();
    }
    
    private void Update()
    {
    }
    
    [HarmonyPatch(typeof(ControllerManager), nameof(ControllerManager.Update))]
    public static void UpdatePatch() => Instance.Update();
}

public class ShipAction
{
    public ShipAction()
    {
        
    }
    
    public void Start()
    {
        ControlManager.Instance.allShipActions.Add(this);
    }
}