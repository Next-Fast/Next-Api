using System;
using System.Collections.Generic;
using NextShip.Buttons;

namespace NextShip.Roles.Core;

public class RoleAction
{
    public List<ButtonBase> Buttons { get; set; }
    public List<Action> Actions { get; set; }
    
    public RoleAction()
    {
        
    }
}