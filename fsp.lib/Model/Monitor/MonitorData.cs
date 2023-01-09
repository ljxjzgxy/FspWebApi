using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using fsp.lib.ValueObject;

namespace fsp.lib.Model.Monitor;
public class MonitorData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = "";
    public string ServiceName { get; set; } = "";
    public string ContainerID { get; set; } = "";
    public string IpAddress { get; set; } = "";
    public DateTime LaunchTime { get; set; }
    public DateTime LastCheckTime { get; set; }
    public ServiceStatus Status { get; set; } 
}
