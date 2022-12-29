using System.Runtime.CompilerServices;
using fsp.lib.Model.logging;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace fsp.lib.Logging;

public class MongoDbLoggingService : IMongoDbLoggingService
{
    private readonly IMongoCollection<LoggingData> _loggingsCollection;
    public MongoDbLoggingService(IOptionsMonitor<MongoDbLoggerConfiguration> config)
    {
        var mongoClient = new MongoClient(config.CurrentValue.database.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(config.CurrentValue.database.DbName);
        _loggingsCollection = mongoDatabase.GetCollection<LoggingData>(config.CurrentValue.database.CollectionName);
    }

    public async Task<List<LoggingData>> GetAsync() =>
        await _loggingsCollection.Find(_ => true).ToListAsync();

    public async Task<LoggingData?> GetAsync(string UserId) =>
        await _loggingsCollection.Find(x => x.UserId == UserId).FirstOrDefaultAsync();

    public async Task<List<LoggingData>> GetAsync(LoggingRequest request)
    {
        var builder = Builders<LoggingData>.Filter;
        var filter = builder.Empty;


        if (request.DateStart != null) filter &= builder.Gte(x => x.LogDate, request.DateStart.Value.ToUniversalTime());
        if (request.DateEnd != null) filter &= builder.Lte(x => x.LogDate,request.DateEnd.Value.ToUniversalTime());
        if (!string.IsNullOrEmpty(request.Loglevel)) filter &= builder.Eq(x => x.Loglevel, request.Loglevel);
        if (!string.IsNullOrEmpty(request.Loglevel)) filter &= builder.Eq(x => x.Loglevel, request.Loglevel);
        if (!string.IsNullOrEmpty(request.Environment)) filter &= builder.Eq(x => x.Environment, request.Environment);
        if (!string.IsNullOrEmpty(request.Service)) filter &= builder.Eq(x => x.Service, request.Service);
        if (!string.IsNullOrEmpty(request.Category)) filter &= builder.Eq(x => x.Category, request.Category);
        if (!string.IsNullOrEmpty(request.UserId)) filter &= builder.Eq(x => x.UserId, request.UserId);
        if (!string.IsNullOrEmpty(request.IPAddress)) filter &= builder.Regex(nameof(request.IPAddress), request.IPAddress);
        if (!string.IsNullOrEmpty(request.RequestAddress)) filter &= builder.Regex(nameof(request.RequestAddress), request.RequestAddress);
        if (!string.IsNullOrEmpty(request.Routes)) filter &= builder.Regex(nameof(request.Routes), request.Routes);
        if (!string.IsNullOrEmpty(request.UserAgent)) filter &= builder.Regex(nameof(request.UserAgent), request.UserAgent);
        if (!string.IsNullOrEmpty(request.Message)) filter &= builder.Regex(nameof(request.Message), request.Message);

        var SkipCount = (request.PageIndex - 1) * request.PageSize;

        var sort = Builders<LoggingData>.Sort.Descending("LogDate");  
        return await _loggingsCollection.Find(filter).Skip(SkipCount).Limit(request.PageSize).Sort(sort).ToListAsync();
    }

    public async Task CreateAsync(LoggingData loggingData) =>
        await _loggingsCollection.InsertOneAsync(loggingData);

    public async Task UpdateAsync(string id, LoggingData updateLog) =>
        await _loggingsCollection.ReplaceOneAsync(x => x.UserId == id, updateLog);

    public async Task RemoveAsync(string id) =>
        await _loggingsCollection.DeleteOneAsync(x => x.UserId == id);

}
