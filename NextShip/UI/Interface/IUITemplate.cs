using UnityEngine;

namespace NextShip.UI.Interface;

public interface IUITemplate
{
    public GameObject GameObject { get; set; }
    public string Name { get; set; }

    public IUITemplate Generate();

    public IUITemplate Get(string name);

}