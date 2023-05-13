using System;
using System.Collections.Generic;

namespace Laba2;

public partial class Match
{
    public int Id { get; set; }

    public int StadiumId { get; set; }

    public int DivisionId { get; set; }

    public DateTime Date { get; set; }

    public virtual Division Division { get; set; } = null!;

    public virtual ICollection<Participate> Participates { get; set; } = new List<Participate>();

    public virtual Stadium Stadium { get; set; } = null!;
}
