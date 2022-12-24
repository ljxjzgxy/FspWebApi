using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fsp.lib.crypto;
using NpgsqlTypes;

namespace fsp.lib.Postgresql.Users;
public class UsersService : IUsersService
{
    private readonly IDbPostgresql _dbPostgresql;
    private readonly ICrypto _crypto;

    public UsersService(IDbPostgresql dbPostgresql, ICrypto crypto)
    {
        _dbPostgresql = dbPostgresql;
        _crypto = crypto;
    }

    public async Task<bool> CheckPassword(string UserId, string Password)
    {
        var hashedPassword = _crypto.SHA256Encrypt(Password);

        await _dbPostgresql.Prepare();
        _dbPostgresql.SetCommandSql("SELECT count(*) FROM users WHERE lower(userid) = @pUserId and password_hash = @pPasswordHash");
        _dbPostgresql.AddParameter("pUserId", NpgsqlDbType.Varchar, UserId.ToLower());
        _dbPostgresql.AddParameter("pPasswordHash", NpgsqlDbType.Varchar, hashedPassword);

        var count = await _dbPostgresql.ExecuteScalarAsync();

        return (count != null) && (long)count == 1;
    }
}
