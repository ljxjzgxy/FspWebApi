
namespace fsp.lib.Model.session;
public class SessionRequest
{
    public string? UserId { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public int ttl { get; set; }
}
