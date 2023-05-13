using System;
using System.Collections.Generic;

namespace Laba2;

public partial class Club
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public int ManagerId { get; set; }

    public virtual Manager Manager { get; set; } = null!;

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
