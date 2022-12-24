using fsp.lib;
using fsp.lib.Model.session;
using fsp.lib.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace session.svc.pri.Controllers.v1;

public class SessionController : ApiControllerBaseV1
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResult<SessionResponse>>> QuerySession(string UserId)
    {
        var sessionData = await _sessionService.QuerySession(UserId);

        return Ok(
            new ApiResult<SessionResponse>
            {
                Success = true,
                Data = sessionData == null ? null : new SessionResponse { Token = sessionData.Token, RefreshToken = sessionData.RefreshToken }
            }
        );
    }

    [HttpPost]
    public async Task<ActionResult<ApiResultBase>> SaveSession(SessionRequest request)
    {
        var sessionData = new SessionData();
        sessionData.UserId = request.UserId;
        sessionData.Token = request.Token;
        sessionData.RefreshToken = request.RefreshToken;
        sessionData.ttl = request.ttl;
        return Ok( new ApiResultBase
        {
            Success = await _sessionService.SaveSession(sessionData),
        });
    }
}
