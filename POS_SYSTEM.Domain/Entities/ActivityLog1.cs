using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class ActivityLog1
{
    public long LogId { get; set; }

    public int UserId { get; set; }

    public string Module { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string? Description { get; set; }

    public string? Ipaddress { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
