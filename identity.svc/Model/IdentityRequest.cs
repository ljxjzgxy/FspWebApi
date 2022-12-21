using System.ComponentModel.DataAnnotations;

namespace identity.svc.Model;

public class identityRequest
{
    [Required]
    public string? UserId { get; set; }

    [Required]
    public string? Password { get; set;}
}
