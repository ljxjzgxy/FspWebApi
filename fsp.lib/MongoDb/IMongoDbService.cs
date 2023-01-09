using fsp.lib.Model.Monitor;

namespace fsp.lib.MongoDb;

public interface IMongoDbService
{
    Task Save<T>(T Data, string TableName);
    Task<List<MonitorData>> GetAllServiceMonitorDataAsync(string TableName);
    Task UpsertMonitorData(MonitorData Data, string TableName);
}