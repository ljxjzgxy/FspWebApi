using fsp.lib;
using fsp.lib.Consts;
using fsp.lib.Model.logging;
using fsp.lib.Model.Monitor;
using fsp.lib.MongoDb;
using Microsoft.AspNetCore.Mvc;

namespace monitor.svc.pri.Controllers.v1;

public class MonitorController : ApiControllerBaseV1
{
    private readonly IMongoDbService _mongoDbService;

    public MonitorController(IMongoDbService mongoDbService)
    {
        _mongoDbService = mongoDbService;
    }
 

    [HttpPut]
    public async Task<ActionResult<ApiResult<List<MonitorData>>>> FetchServicesMonitorData() {
        var servicesMonitorData = await _mongoDbService.GetAllServiceMonitorDataAsync(MongoDbTable.Monitor);

        return new ApiResult<List<MonitorData>> { 
            Success= true,
            Data = servicesMonitorData
        };         
    } 
}
