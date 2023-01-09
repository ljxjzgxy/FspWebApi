
using fsp.lib.Model.session;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace fsp.lib.Session;
public class SessionService : ISessionService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly int _sessionDbIndex = 0;
    private const string _preFix = "session_";
    public SessionService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<SessionData?> QuerySession(string UserId)
    {
        var key = _preFix + UserId.ToLower();
        var db = _redis.GetDatabase(_sessionDbIndex);
        var data = await db.StringGetAsync(key);

        return data.HasValue ? JsonConvert.DeserializeObject<SessionData>(data) : null;
    }

    public async Task<bool> SaveSession(SessionData sessionData)
    {
        if (sessionData != null && sessionData.UserId !=null)
        {
            var key = _preFix + sessionData.UserId.ToLower();
            var db = _redis.GetDatabase(_sessionDbIndex);
            return await db.StringSetAsync(key, JsonConvert.SerializeObject(sessionData));
            
        }
        return false;
    }
}
