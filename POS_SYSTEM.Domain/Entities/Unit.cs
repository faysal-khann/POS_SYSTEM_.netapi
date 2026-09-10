using System;
using System.Collections.Generic;

namespace POS_SYSTEM.Domain.Entities;

public partial class Unit
{
    public int UnitId { get; set; }

    public string UnitName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? ShortName { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
