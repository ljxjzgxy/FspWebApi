using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsp.lib.Postgresql.Users;
public interface IUsersService
{
    public Task<bool> CheckPassword(string UserId, string Password);
}
