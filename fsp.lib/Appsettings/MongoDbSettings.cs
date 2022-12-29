using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsp.lib.Appsettings;
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = "";
    public string DbName { get; set; } = "";
    public string CollectionName { get; set; } = "";
}
