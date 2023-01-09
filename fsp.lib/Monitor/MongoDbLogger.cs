using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Versioning;
using fsp.lib.Appsettings;
using fsp.lib.Model.logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace fsp.lib.Logging; 
public sealed class MongoDbLoggerConfiguration
{
    public int EventId { get; set; }
    public Dictionary<string, LogLevel> logLevel { get; set; } = new();
    public MongoDbSettings database { get; set; } = new();
}

[UnsupportedOSPlatform("browser")]
[ProviderAlias("MongoDb")]
public sealed class MongoDbLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeToken;
    private MongoDbLoggerConfiguration _currentConfig;
    private readonly ConcurrentDictionary<string, MongoDbLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
    private readonly IMongoDbLoggingService _mongoDbLoggingService;
    private readonly string _assemblyName;
    public MongoDbLoggerProvider(
        IOptionsMonitor<MongoDbLoggerConfiguration> config, IServiceProvider serviceProvider)
    {
        _currentConfig = config.CurrentValue;
        _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
        _mongoDbLoggingService = serviceProvider.GetRequiredService<IMongoDbLoggingService>();
        var assembly = Assembly.GetEntryAssembly();
        if (assembly == null) _assemblyName = "Unknown Assembly";
        else _assemblyName =  $"{assembly.GetName().Name}";
    }

    public ILogger CreateLogger(string categoryName) =>
        _loggers.GetOrAdd(categoryName, name => new MongoDbLogger(name, GetCurrentConfig, _mongoDbLoggingService, _assemblyName));

    private MongoDbLoggerConfiguration GetCurrentConfig() => _currentConfig;

    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }
}


public sealed class MongoDbLogger : ILogger
{
    private readonly string _name;
    private readonly Func<MongoDbLoggerConfiguration> _getCurrentConfig;
    private readonly IMongoDbLoggingService _mongoDbLoggingService;
    private readonly string _assemblyName;

    public MongoDbLogger(string name, Func<MongoDbLoggerConfiguration> getCurrentConfig, IMongoDbLoggingService mongoDbLoggingService,string assemblyName)
    {
        (_name, _getCurrentConfig, _mongoDbLoggingService, _assemblyName) = (name, getCurrentConfig, mongoDbLoggingService, assemblyName);
    }


    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default!;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        var config = _getCurrentConfig();

        if (config.logLevel.Count == 0 && logLevel >= LogLevel.Information) return true;
        if (config.logLevel.Any(x => x.Key == _name && x.Value <= logLevel)) return true;
        if (config.logLevel.Any(x => x.Key == "Default" && x.Value <= logLevel)) return true;

        return false;
    }


    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        MongoDbLoggerConfiguration config = _getCurrentConfig();
        if (config.EventId == 0 || config.EventId == eventId.Id)
        {
            var loggingData = new LoggingData();
            var message = $"{formatter(state, exception)}";
            var fields = message.Split("█");
          
            loggingData.Loglevel = $"{logLevel}";
            loggingData.Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")??"unset";
            loggingData.Service = _assemblyName;
            loggingData.Category = _name;
            if(fields.Length  > 1)
            {  
                loggingData.UserId = fields[0];
                loggingData.IPAddress = fields[1];
                loggingData.RequestAddress = fields[2];
                loggingData.Routes = fields[3];
                loggingData.UserAgent = fields[4];              
            }
            else
            {
                loggingData.Message = message;
            }

            if (exception != null)
            {
                var preMess = string.IsNullOrWhiteSpace(loggingData.Message) ? "" : $"***{ loggingData.Message}***,"; 
                var exceptionMess = $"Exception: {exception.Message} ---> {exception.StackTrace}";
                loggingData.Message = string.IsNullOrWhiteSpace(preMess) ? exceptionMess : preMess + exceptionMess;
            }


            loggingData.LogDate = DateTime.Now;

            _mongoDbLoggingService.CreateAsync(loggingData).Wait();

            //Console.WriteLine($"[{eventId.Id,2}: {logLevel,-12}]");

            //Console.Write($" >>> Custom Logging(2) <<<  {_name} - ");

            //Console.Write($"{formatter(state, exception)}");
        }
    }
}

