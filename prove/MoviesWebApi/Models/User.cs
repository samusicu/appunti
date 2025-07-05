using System;
using System.Collections.Generic;

namespace MoviesWebApi.Models;

public partial class User
{
    public byte UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
