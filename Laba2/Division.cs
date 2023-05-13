using System;
using System.Collections.Generic;

namespace Laba2;

public partial class Division
{
    public int Id { get; set; }

    public bool DivisoinOrLeague { get; set; }

    public byte Level { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}
