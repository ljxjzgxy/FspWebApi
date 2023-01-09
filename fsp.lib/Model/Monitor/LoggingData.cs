using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace fsp.lib.Model.logging;
public class LoggingData
{
    [BsonId]
    //[BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Loglevel { get; set; } = "";
    public string Environment { get; set; } = "";
    public string Service { get; set; } = "";
    public string Category { get; set; } = "";
    public string UserId { get; set; } = "";   
    public string IPAddress { get; set; } = "";
    public string RequestAddress { get; set; } = "";
    public string Routes { get; set; } = "";
    public string UserAgent { get; set; } = "";
    public string Message { get; set; } = "";


    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime LogDate { get; set; }
}
