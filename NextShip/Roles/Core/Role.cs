using System;
using System.Collections.Generic;
using NextShip.Utilities.Attributes;

namespace NextShip.Roles.Core;

public class Role
{
    protected Type RoleType;
    
    protected Type RoleBaseType;

    protected SimpleRoleInfo SimpleRoleInfo;
    
    public List<RoleBase> RoleBase { get; set; }
    
    public List<PlayerControl> PlayerControls { get; set; }
    
    public RoleAction RoleAction { get; set; }
    
    public RoleEvent RoleEvent { get; set; }
    
    public Func<PlayerControl, RoleBase> CreateRoleBase { get; set; }
    
    public virtual void OptionLoad()
    {}
}