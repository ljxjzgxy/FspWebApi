using Microsoft.AspNetCore.Mvc;
using fsp.lib;
using fsp.lib.crypto;
using fsp.lib.Model.identity;
using fsp.lib.Configuration;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using fsp.lib.Postgresql.Users;
using fsp.lib.Jwt;

namespace identity.svc.Controllers.v1;

public class IdentityController : ApiControllerBaseV1
{
    private readonly IUsersService _usersService;
    private readonly IJwtService _jwtService;
    
    public IdentityController(IUsersService usersService,IJwtService jwtService)
    {
        _usersService = usersService;
        _jwtService = jwtService;      
    }

    [HttpPost]
    public async Task<ActionResult<ApiResult<IdentityResponse>>> IssueToken(IdentityRequest request)
    {
        var UserId = request.UserId??"";
        var Password = request.Password??"";

        if (await _usersService.CheckPassword(UserId, Password!))
        {
            IdentityResponse response = new IdentityResponse();
            response.token = _jwtService.GenerateToken(UserId);

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

