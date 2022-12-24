using Npgsql;
using NpgsqlTypes;

namespace fsp.lib.Postgresql;
public interface IDbPostgresql
{
    public Task Prepare(string sql = "");
    public void SetCommandSql(string sql, bool clearParameter = false);
    public void AddParameter(string ParaName, NpgsqlDbType npgDbType, object value);
    public Task<NpgsqlDataReader> ExecuteReaderAsync();
    public Task<object?> ExecuteScalarAsync();
    public Task<int> ExecuteNonQueryAsync();

}
