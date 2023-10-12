using System;
using System.Collections.Generic;

namespace NextShip.Roles.Core;

public class Role
{
    protected Type RoleBaseType;
    protected Type RoleType;

    public SimpleRoleInfo SimpleRoleInfo { get; protected set; }

    public List<RoleBase> RoleBase { get; set; }

    public List<PlayerControl> PlayerControls { get; set; }

    public RoleAction RoleAction { get; set; }

    public RoleEvent RoleEvent { get; set; }
    
    public bool EnableAssign { get; set; }

    public Func<PlayerControl, RoleBase> CreateRoleBase { get; set; }

    public virtual void OptionLoad()
    {
    }

    public virtual bool CanCreate()
    {
        return true;
    }
}