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
    private readonly IHttpUtil _httpUtil;
    private readonly ServiceUriInternal _serviceUriInternal;

    public IdentityController(IUsersService usersService,IJwtService jwtService,IHttpUtil httpUtil,IOptions<ServiceUriInternal> optServiceUriInternal)
    {
        _usersService = usersService;
        _jwtService = jwtService;
        _httpUtil = httpUtil;
        _serviceUriInternal = optServiceUriInternal.Value;
    }

    [HttpPut]
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
            await _httpUtil.Post<SessionData>(_serviceUriInternal.SessionSvc!,sessionData);

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
            return NotFound(new ApiResult<string> { Code = StatusCodes.Status404NotFound, Success = false,Message ="invalid" });
        }
    }
}

