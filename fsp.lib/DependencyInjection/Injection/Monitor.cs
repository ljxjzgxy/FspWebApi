using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using fsp.lib.Appsettings;
using fsp.lib.Consts;
using fsp.lib.Model.Monitor;
using fsp.lib.MongoDb;
using fsp.lib.ValueObject;
using Microsoft.Extensions.Configuration;

namespace fsp.lib.DependencyInjection.Individual;
public static class Monitor
{
    public static async Task<IServiceCollection> AddCustomMonitor(this IServiceCollection services, IConfiguration config, string assemblyName)
    {

        var mongoDbSettings = new MongoDbSettings();
        config.Bind(nameof(MongoDbSettings), mongoDbSettings);
        //register service to monitor
        var containerId = Dns.GetHostName();
        var ip = Dns.GetHostEntry(containerId).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
        var ipAddr = ip == null ? "" : ip.ToString();
        MonitorData monitorData = new()
        {
            ServiceName = assemblyName,
            ContainerID = containerId,
            IpAddress = ipAddr,
            LaunchTime = DateTime.Now,
            LastCheckTime = DateTime.Now,
            Status = ServiceStatus.Starting
        };
        var mongoDbService = new MongoDbService(mongoDbSettings);
        await mongoDbService.UpsertMonitorData(monitorData, MongoDbTable.Monitor);

        services.AddSingleton<IMongoDbService>(mongoDbService);

        return services;
    }
}
