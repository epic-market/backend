using EpicMarket.Business.API.Helpers;
using EpicMarket.Contracts;
using EpicMarket.Data.Models;
using EpicMarket.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Configuration;

namespace EpicMarket.Business.API.Extension
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApplicationConfigurationService, ApplicationConfigurationService>();
            services.AddScoped<ICommunicationService, CommunicationService>();
            services.AddScoped<ICommunicationQueueService, CommunicationQueueService>();
            services.AddScoped<IEventLogService, EventLogService>();
            services.AddScoped<IBusinessService, BusinessService>();
            services.AddScoped<IStaticService, StaticService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IFileService,FileService>();
            services.AddScoped<LogUserActivity>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            Serilog.Log.Logger = new LoggerConfiguration()
                            .WriteTo.Console()
                            .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
                            .Enrich.WithThreadId()
                            .Enrich.WithThreadName()
                            .Enrich.FromLogContext()
                           ////https://stackoverflow.com/questions/63873570/serilog-file-name-utc-date-format
                           .WriteTo.File(
                                           "Logs/epicmarketAPI-.txt",
                                           rollingInterval: RollingInterval.Hour,
                                           fileSizeLimitBytes: 100000,
                                           outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{UserName}]-[{CorrelationId}] {Message:lj} {Exception} {NewLine}")
                           .Enrich.WithCorrelationId()
            /*Install-Package Serilog.Sinks.MSSqlServer 
             Here we are simply specifying a local SQL instance as the target, with a table name of “Logs”. 
             Make sure you update the connection string to fit your needs, and you’ll need to create the LoggingDb database if it doesn’t exist. 
             */
                           .WriteTo.MSSqlServer(
                                   config.GetConnectionString("DefaultConnection"),
                       new MSSqlServerSinkOptions
                       {
                           TableName = "EpicMarketLogs",
                           SchemaName = "dbo",
                           AutoCreateSqlTable = true
                       }, restrictedToMinimumLevel: LogEventLevel.Warning)
                           /*These are the events emitted for a single request to the homepage. The Serilog.AspNetCore package, which we installed at the beginning of this article,
                            helps condense these log events into more manageable information.
                            First, we need to override the default log level for the Microsoft.AspNet logger in our logger config:*/
                           .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                       //.WriteTo.ApplicationInsights(new TelemetryConfiguration { InstrumentationKey = config["ApplicationInsights:Key"] }, TelemetryConverter.Traces)
                           .CreateLogger();
            Serilog.Log.Information("Starting web job");
            services.AddSingleton(Serilog.Log.Logger);
            return services;
        }
    }
}
