﻿using fsp.lib.Appsettings;
using fsp.lib.Consts;
using fsp.lib.Model.Monitor;
using fsp.lib.MongoDb;
using fsp.lib.ValueObject;
using System.Net.Sockets;
using System.Net;
using fsp.lib.HttpClient;
using Microsoft.AspNetCore.SignalR;
using monitor.svc.pri.SignalRHub;

namespace monitor.svc.pri;

public class ServicesMonitorDaemon : BackgroundService
{  
    private readonly IMongoDbService _mongoDbService;
    private readonly IHttpUtil _httpUtil;
    private readonly IHubContext<ServicesMonitorHub> _hubContext;
    public ServicesMonitorDaemon(IMongoDbService mongoDbService, IHttpUtil httpUtil, IHubContext<ServicesMonitorHub> hubContext)
    {
        _mongoDbService = mongoDbService;
        _httpUtil = httpUtil;
        _hubContext = hubContext;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(60000 * 5);
            var servicesMonitorData = await _mongoDbService.GetAllServiceMonitorDataAsync(MongoDbTable.Monitor);

            foreach (var serviceMonitorData in servicesMonitorData)
            {
                serviceMonitorData.LastCheckTime = DateTime.Now;

                if (serviceMonitorData.IpAddress == null)
                {
                    serviceMonitorData.Status = ServiceStatus.Offline;
                }
                else
                {
                    var serviceHealthUri = $"http://{serviceMonitorData.IpAddress}/health";
                    var response = await _httpUtil.Get(serviceHealthUri);
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        serviceMonitorData.Status = ServiceStatus.Online;
                    }
                    else
                    {
                        serviceMonitorData.Status = ServiceStatus.Offline;
                    }

                    await _mongoDbService.UpsertMonitorData(serviceMonitorData, MongoDbTable.Monitor);
                }
            }

            await _hubContext.Clients.All.SendAsync("ClientServicesMonitorDataUpdate", servicesMonitorData);
        }
    }
}
