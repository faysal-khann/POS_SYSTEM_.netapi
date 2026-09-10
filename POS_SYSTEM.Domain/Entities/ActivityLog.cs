using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class ActivityLog
{
    public long LogId { get; set; }

    public int? UserId { get; set; }

    public string ActionType { get; set; } = null!;

    public string? Module { get; set; }

    public string Description { get; set; } = null!;

    public string? Ipaddress { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
