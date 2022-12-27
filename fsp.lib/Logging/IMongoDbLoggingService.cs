using fsp.lib.Model.logging;

namespace fsp.lib.Logging;
 
    public interface IMongoDbLoggingService
    {
        Task CreateAsync(LoggingData loggingData);
        Task<List<LoggingData>> GetAsync();
        Task<LoggingData?> GetAsync(string id);
        Task RemoveAsync(string id);
        Task UpdateAsync(string id, LoggingData updateLog);
    }
 