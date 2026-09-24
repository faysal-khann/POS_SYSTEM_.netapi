using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Infrastructure.Persistence.Entities;

public partial class Permission
{
    public int PermissionId { get; set; }

    public int? ParentPermissionId { get; set; }

    public string PermissionKey { get; set; } = null!;

    public string PermissionName { get; set; } = null!;

    public string? Description { get; set; }

    public string Module { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int SortOrder { get; set; }

    public virtual ICollection<Permission> InverseParentPermission { get; set; } = new List<Permission>();

    public virtual Permission? ParentPermission { get; set; }

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}
