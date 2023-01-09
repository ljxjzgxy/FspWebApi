
using fsp.lib.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace fsp.lib.DependencyInjection.Individual;
public static class MongoDbLoggerExtensions
{
    public static ILoggingBuilder AddCustomMongoDbLogger(
        this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, MongoDbLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions<MongoDbLoggerConfiguration, MongoDbLoggerProvider>(builder.Services);

        builder.Services.AddSingleton<IMongoDbLoggingService, MongoDbLoggingService>();

        return builder;
    }

    //public static ILoggingBuilder AddMongoDbLogger(this ILoggingBuilder builder, Action<MongoDbLoggerConfiguration> configure)
    //{
    //    builder.AddMongoDbLogger();
    //    builder.Services.Configure(configure);

    //    return builder;
    //}
}

