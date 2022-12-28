using Microsoft.AspNetCore.Mvc;
using fsp.lib;
using fsp.lib.crypto;
using fsp.lib.Model.identity;
using System.Security.Claims;
using fsp.lib.Postgresql.Users;
using fsp.lib.Jwt;
using fsp.lib.HttpClient;
using fsp.lib.Model.session;
using Microsoft.Extensions.Options;
using fsp.lib.Appsettings;
using Microsoft.OpenApi.Any;

namespace identity.svc.Controllers.v1;

public class IdentityController : ApiControllerBaseV1
{
    private readonly IUsersService _usersService;
    private readonly IJwtService _jwtService;
    private readonly IApiHelper _apiHelper;
    private readonly ServiceUriInternal _serviceUriInternal;

    public IdentityController(IUsersService usersService,IJwtService jwtService,IApiHelper apiHelper,IOptions<ServiceUriInternal> optServiceUriInternal)
    {
        _usersService = usersService;
        _jwtService = jwtService;
        _apiHelper = apiHelper;
        _serviceUriInternal = optServiceUriInternal.Value;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResult<IdentityResponse>>> IssueToken(IdentityRequest request)
    {
        var UserId = request.UserId??"";
        var Password = request.Password??"";       

        if (request?.UserId !=null &&  await _usersService.CheckPassword(UserId, Password!))
        {           
            var token = _jwtService.GenerateToken(UserId);

            SessionData sessionData = new SessionData();
            sessionData.UserId = UserId;
            sessionData.Token = token;
            await _apiHelper.PostApi<SessionData>(_serviceUriInternal.SessionSvc!,sessionData);

            IdentityResponse response = new IdentityResponse();
            response.token = token;      

            return new ApiResult<IdentityResponse>()
            {
                Success = true,
                Data = response
            };
        }
        else
        {
            return new BadRequestResult();
        }
    }
}

