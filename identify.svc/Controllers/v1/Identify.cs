using Microsoft.AspNetCore.Mvc;
using fsp.lib;
using Microsoft.Extensions.Options;
using fsp.lib.Configuration;

namespace identify.svc.Controllers.v1;

public class IdentifyController : ApiControllerBaseV1
{
    [HttpGet]
    public ActionResult<ApiResult<string>> GetToken([FromServices] IOptions<JwtOptions> _jwtOption)
    {
        return new ApiResult<string>()
        {
            Success = true,
            Message = "test auto deploy swagger 500",
            Data = _jwtOption.Value.Key

        };
    }
}

