using System;
using System.Collections.Generic;

namespace MoviesWebApi.Models;

public partial class Rating
{
    public byte UserId { get; set; }

    public int MovieId { get; set; }

    public double Rating1 { get; set; }

    public int Timestamp { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
