using System;
using System.Collections.Generic;

namespace MoviesWebApi.Models;

public partial class Movie
{
    public int MovieId { get; set; }

    public string Title { get; set; } = null!;

    public short? Year { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
