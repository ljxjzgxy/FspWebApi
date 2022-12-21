using Microsoft.AspNetCore.Mvc;
using fsp.lib;
using identity.svc.Model;
using fsp.lib.crypto;

namespace identity.svc.Controllers.v1;

public class IdentityController : ApiControllerBaseV1
{
   
    [HttpPost]
    public ActionResult<ApiResult<identityResponse>> IssueToken([FromServices] ICrypto crypto , identityRequest request)
    {
        identityResponse response  = new identityResponse();

        var pwd = crypto.SHA256Encrypt(request.Password!);

        response.token = "lwejrlwerw";

        return new ApiResult<identityResponse>()
        {
            Success = true,          
            Data = response
        };
    }
}

