using System.Collections.Generic;

namespace NextShip.Buttons;

public class ButtonsManager
{
    public List<ButtonBase> _AllButtons = [];
    public List<RoleButton> _AllRoleButton = [];
    public static ButtonsManager Instance { get; private set; }

    public static ButtonsManager Get()
    {
        return Instance ??= new ButtonsManager();
    }
}