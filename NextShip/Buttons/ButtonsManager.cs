using System.Collections.Generic;

namespace NextShip.Buttons;

public class ButtonsManager
{
    public static ButtonsManager Instance { get; private set; }
    
    public List<ButtonBase> _AllButtons = new();
    public List<RoleButton> _AllRoleButton = new();

    public static ButtonsManager Get() => Instance ??= new ButtonsManager();
}