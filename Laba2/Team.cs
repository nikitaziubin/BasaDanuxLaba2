using System;
using System.Collections.Generic;

namespace Laba2;

public partial class Team
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ClubId { get; set; }

    public virtual Club? Club { get; set; }

    public virtual ICollection<Participate> Participates { get; set; } = new List<Participate>();
}
