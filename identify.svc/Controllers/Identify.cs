using fsp.lib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace identify.svc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Identify : ControllerBase
    {
        [HttpGet]
        public ActionResult<ApiResult<String>>  GetToken()
        {
            return new ApiResult<String>()
            {
                Success = true,
                Data = new String("Test 002")

            };
        }
    }
}
