using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NextShip.Manager;

public static class PositionManager
{
    public static readonly List<GameObject> AllGameObjects = AllGameObjectsPosition.Keys.ToList();
    public static Dictionary<GameObject, List<Vector3>> AllGameObjectsPosition = new Dictionary<GameObject, List<Vector3>>();

    public static void Set(this GameObject objet, Vector3 vector3)
    {
        objet.transform.position = vector3;
        if(AllGameObjectsPosition.ContainsKey(objet)) { AllGameObjectsPosition[objet].Add(vector3); return;}
        AllGameObjectsPosition.Add(objet, new List<Vector3> { vector3 });
    }

    public static Vector3 Left = new Vector3();
}