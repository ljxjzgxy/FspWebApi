using fsp.lib.Swagger;

namespace fsp.lib.DependencyInjection
{
    public static class Swagger
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, string AssemblyName = "", string Title = "", string Description = "")
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Default;
                //options.ReportApiVersions = true;
                //options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(verison =>
                {
                    c.SwaggerDoc(verison, new OpenApiInfo()
                    {
                        Title = string.IsNullOrWhiteSpace(Title) ? $"{AssemblyName} - {verison}" : $"{Title} - {verison}",
                        Version = verison,
                        Description = string.IsNullOrWhiteSpace(Description) ? $"Swagger API - {AssemblyName}" : Description
                    });
                });


                var xmlCommentsFile = Path.Combine(AppContext.BaseDirectory, $"{AssemblyName}.xml");
                if (!string.IsNullOrWhiteSpace(xmlCommentsFile) && File.Exists(xmlCommentsFile))
                {
                    c.IncludeXmlComments(xmlCommentsFile);
                }

                c.OrderActionsBy(o => o.RelativePath);

                // This call remove version from parameter, without it we will have version as parameter 
                // for all endpoints in swagger UI
                c.OperationFilter<RemoveVersionParameterFilter>();
                // This make replacement of v{version:apiVersion} to real version of corresponding swagger doc.
                c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();


                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "Please enter token",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey,
                //    BearerFormat = "JWT",
                //    Scheme = "Bearer"
                //});

                //c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                //    {
                //        new OpenApiSecurityScheme {
                //            Reference = new OpenApiReference() {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        new string[]{}
                //    }
                //});

                c.OperationFilter<FileUploadFilter>();
            });

            return services;
        }


        public static IApplicationBuilder UseCustomSwaggerUI(this IApplicationBuilder app)
        {
            return app.UseSwaggerUI(options =>
            {
                foreach (var version in typeof(ApiVersions).GetEnumNames())
                {
                    options.SwaggerEndpoint($"api/{version}/swagger.json", $"{version}");
                }
            });
        }
    }
}
