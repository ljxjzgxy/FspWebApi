﻿using System.Text.Json.Serialization;
using fsp.lib.DependencyInjection.Individual;
using fsp.lib.HttpClient;

namespace fsp.lib.DependencyInjection;
public static class InjectEssentialCustomServices
{
    public static async Task<WebApplicationBuilder> AddEssentialCustomServices(this WebApplicationBuilder builder,string assemblyName)
    {
        builder.Services.AddControllers(); //.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        builder.Services.AddCustomSwagger(assemblyName);
        builder.Logging.AddCustomMongoDbLogger();
        builder.Services.AddHealthChecks();
        builder.Services.AddSingleton<IHttpUtil, HttpUtil>();
        await builder.Services.AddCustomMonitor(builder.Configuration, assemblyName);

        return builder;
    }
}