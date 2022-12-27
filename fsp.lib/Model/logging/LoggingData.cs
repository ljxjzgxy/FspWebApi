using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsp.lib.Model.logging;
public class LoggingData
{
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
    public DateTime LogDate { get; set; }
}
