using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsp.lib.Model.session;
public class SessionData
{
    public string? UserId { get; set; }

    public string? Token { get; set; }

    public string? RefreshToken { get; set; }

    public int ttl { get; set;}
}
