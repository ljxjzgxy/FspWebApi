namespace fsp.lib.Configuration;
public class JwtSettings
{
    public string? Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpiredInMin { get; set; }
    public string? EncryptKey { get; set; }

}
