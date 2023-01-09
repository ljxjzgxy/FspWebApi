using System.Collections;
using System.Net;
using fsp.lib.Appsettings;
using fsp.lib.Model.logging;
using fsp.lib.Model.Monitor;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SharpCompress.Common;

namespace fsp.lib.MongoDb;
public class MongoDbService : IMongoDbService
{
    private readonly IMongoDatabase _mongoDatabase;

    public MongoDbService(MongoDbSettings mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
        _mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.DbName);
    }

    public async Task Save<T>(T Data, string TableName)
    {
        var collectionName = _mongoDatabase.GetCollection<T>(TableName);
        await collectionName.InsertOneAsync(Data);
    }

    public async Task<List<MonitorData>> GetAllServiceMonitorDataAsync(string TableName)
    {
        var collectionName = _mongoDatabase.GetCollection<MonitorData>(TableName);
        var filter = Builders<MonitorData>.Filter.Where(x => true);
        return await collectionName.Find(filter).ToListAsync<MonitorData>();
    }
    public async Task UpsertMonitorData(MonitorData Data, string TableName)
    {
        var collection = _mongoDatabase.GetCollection<MonitorData>(TableName);

        var filter = Builders<MonitorData>.Filter.Where(x => x.IpAddress == Data.IpAddress);

        var result = await collection.Find(filter).ToListAsync<MonitorData>();
        if (result == null || result.Count == 0)
        {
            await collection.InsertOneAsync(Data);
        }
        else
        {        
            if (Data != null)
            {
                var update = Builders<MonitorData>.Update.Set(x => x.LastCheckTime, Data.LastCheckTime)
                .Set(x => x.Status, Data.Status);
                await collection.UpdateOneAsync(filter, update); 
            }
        }
    }
}
