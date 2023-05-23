using System;
using System.Collections.Generic;

namespace Laba2;

public partial class Manager
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
}
