using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Open.Serialization.Json;
using Open.Serialization.Json.Newtonsoft;
using OvertimePolicies;
using Salaries.Application.Services;
using Salaries.Core.Repositories;
using Salaries.Infrastructure.DataAccess.Dapper;
using Salaries.Infrastructure.DataAccess.Dapper.Repositories;
using Salaries.Infrastructure.DataAccess.EF;
using Salaries.Infrastructure.DataAccess.EF.Repositories;
using Salaries.Infrastructure.Exceptions;
using Salaries.Infrastructure.Parsers;
using Shared.Infrastructure.Commands;
using Shared.Infrastructure.Dispatchers;
using Shared.Infrastructure.Queries;

namespace Salaries.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IList<Assembly> assemblies, IConfiguration configuration)
    {
        services.AddJsonSerializer();
        services.AddTransient<ErrorHandlerMiddleware>();
        services.AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>();
        services.AddScoped<ISalaryWriteRepository, SalaryEFRepository>();
        services.AddScoped<ISalaryReadRepository, SalaryDapperRepository>();
        services.AddScoped(typeof(IJsonParser<>), typeof(JsonParser<>));
        services.AddScoped<IXmlParser, XmlParser>();
        services.AddScoped<IOvertimeCalculator, OvertimeCalculator>();
        services.AddSwaggerGen(swagger =>
        {
            swagger.EnableAnnotations();
            swagger.CustomSchemaIds(x => x.FullName);
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Salary API",
                Version = "v1"
            });
        });
        services.AddCommands(assemblies);
        services.AddQueries(assemblies);
        services.AddSingleton<IDispatcher, Dispatcher>();
        
        services.AddDbContext<EFDbContext>(x =>
            x.UseNpgsql(configuration.GetConnectionString("Postgres")));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddSingleton<DapperDbContext>();
        services.AddMvcCore().AddApiExplorer()
            .AddJsonOptions(options => 
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
        });
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlerMiddleware>();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Salary API V1"));
        
        return app;
    }
}