using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fsp.lib.Model.session;
using StackExchange.Redis;

namespace fsp.lib.Session;
public interface ISessionService
{
    public Task<SessionData?> QuerySession(string UserId);

    public Task<bool> SaveSession(SessionData sessionData);
}
