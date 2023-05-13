using System;
using System.Collections.Generic;

namespace Laba2;

public partial class Participate
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public int MatchId { get; set; }

    public int ScoredGoals { get; set; }

    public int BallOwnershipTime { get; set; }

    public string TeamRole { get; set; } = null!;

    public byte? RedCards { get; set; }

    public byte? YellowCards { get; set; }

    public byte? ShotsOnTarget { get; set; }

    public byte? Shots { get; set; }

    public int Passes { get; set; }

    public byte? Offsides { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
