using System.ComponentModel.DataAnnotations;

namespace fsp.lib.Model.identity;

public class IdentityRequest
{
    [Required]
    public string? UserId { get; set; }

    [Required]
    public string? Password { get; set; }
}
