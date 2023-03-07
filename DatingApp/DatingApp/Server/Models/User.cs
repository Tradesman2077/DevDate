using System;
using System.Collections.Generic;

namespace DatingApp.Server.Models;

public partial class User
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Bio { get; set; }

    public string? FavouriteLanguage { get; set; }

    public long? Age { get; set; }

    public string? Matches { get; set; }

    public string? Likes { get; set; }

    public string? Country { get; set; }

    public string? City { get; set; }

    public string? Email { get; set; }

    public string? PhotoUrl { get; set; }
}
