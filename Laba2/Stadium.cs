using System;
using System.Collections.Generic;

namespace Laba2;

public partial class Stadium
{
    public int Id { get; set; }

    public string Adress { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public int MaxCapacity { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}
