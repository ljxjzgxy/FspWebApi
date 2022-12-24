
using fsp.lib.Model.session;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace fsp.lib.Session;
public class SessionService : ISessionService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly int _sessionDbIndex = 0;
    public SessionService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<SessionData?> QuerySession(string UserId)
    {
        var db = _redis.GetDatabase(_sessionDbIndex);
        var data = await db.StringGetAsync(UserId.ToLower());

        return data.HasValue ? JsonConvert.DeserializeObject<SessionData>(data) : null;
    }

    public async Task<bool> SaveSession(SessionData sessionData)
    {
        if (sessionData != null && sessionData.UserId !=null)
        {
            var db = _redis.GetDatabase(_sessionDbIndex);
            return await db.StringSetAsync(sessionData.UserId.ToLower(), JsonConvert.SerializeObject(sessionData));
            
        }
        return false;
    }
}
