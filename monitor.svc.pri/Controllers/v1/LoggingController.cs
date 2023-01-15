using fsp.lib;
using fsp.lib.Logging;
using fsp.lib.Model.logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace monitor.svc.pri.Controllers.v1;

public class LoggingController : ApiControllerBaseV1
{
    private readonly IMongoDbLoggingService _mongoDbLoggingService;
    public LoggingController(IMongoDbLoggingService mongoDbLoggingService)
    {
        _mongoDbLoggingService = mongoDbLoggingService;
    }


    [HttpPut]
    public async Task<ActionResult<ApiResult<List<LoggingData>>>> FetchLoggingData(LoggingRequest request)
    {
        return new ApiResult<List<LoggingData>>{
            Success = true,
            Data = await _mongoDbLoggingService.GetAsync(request)
        };
    }
}
