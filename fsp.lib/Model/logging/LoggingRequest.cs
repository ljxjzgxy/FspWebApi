
using MongoDB.Bson.Serialization.Attributes;

namespace fsp.lib.Model.logging;
public record LoggingRequest
{
    public DateTime? DateStart{ get; set; } 
    public DateTime? DateEnd { get; set; }
    public string? Loglevel { get; set; }
    public string? Environment { get; set; }
    public string? Service { get; set; }
    public string? Category { get; set; }
    public string? UserId { get; set; }
    public string? IPAddress { get; set; }
    public string? RequestAddress { get; set; }
    public string? Routes { get; set; }
    public string? UserAgent { get; set; }
    public string? Message { get; set; }

    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 100;
  
}
