using AutoMapper;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using DnsClient.Internal;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using Radio_Search.Importer.Canada.Data;
using Radio_Search.Importer.Canada.Services;
using Radio_Search.Importer.Canada.Services.Configuration;
using Radio_Search.Importer.Canada.Services.Implementations;
using Radio_Search.Importer.Canada.Services.Interfaces;
using Radio_Search.Importer.Canada.Services.Mappings;
using Radio_Search.Utils.BlobStorage;
using System.Net;
using System.Reflection;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

#region VALUES

#region BOOTSTRAPPING

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production";
var isProduction = string.Equals(environment, "production");

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"{environment}.settings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();
#endregion

#region APP CONFIG

// Common
builder.Configuration.AddAzureAppConfiguration(options =>
{
    var uri = config.GetValue<string>("AppConfig:Common:URL") ?? throw new ArgumentNullException("AppConfig:Common:URL is null");
    var sentinelLabel = config.GetValue<string>("AppConfig:Common:Sentinel") ?? throw new ArgumentNullException("AppConfig:Common:Sentinel is null");
    var refreshInterval = config.GetValue("AppConfig:Common:RefreshInterval", 5);

    options.Connect(new Uri(uri), new DefaultAzureCredential());

    options.Select(KeyFilter.Any, LabelFilter.Null);

    options.ConfigureRefresh(refreshOptions =>
    {
        refreshOptions.Register(sentinelLabel, refreshAll: true);
        refreshOptions.SetRefreshInterval(TimeSpan.FromMinutes(refreshInterval));
    });
});

// Importers
builder.Configuration.AddAzureAppConfiguration(options =>
{
    var uri = config.GetValue<string>("AppConfig:Importer:URL") ?? throw new ArgumentNullException("AppConfig:Importer:URL is null");
    var sentinelLabel = config.GetValue<string>("AppConfig:Importer:Sentinel") ?? throw new ArgumentNullException("AppConfig:Importer:Sentinel is null");
    var keyPrefix = config.GetValue<string>("AppConfig:Importer:Prefix") ?? throw new ArgumentNullException("AppConfig:Importer:Prefix is null");
    var refreshInterval = config.GetValue("AppConfig:Importer:RefreshInterval", 5);

    options.Connect(new Uri(uri), new DefaultAzureCredential());

    options.Select($"{keyPrefix}:*", LabelFilter.Null);
    options.TrimKeyPrefix($"{keyPrefix}:");

    options.ConfigureRefresh(refreshOptions =>
    {
        refreshOptions.Register(sentinelLabel, refreshAll: true);
        refreshOptions.SetRefreshInterval(TimeSpan.FromMinutes(refreshInterval));
    });
});

#endregion

#region KEYVAULT

builder.Configuration.AddAzureKeyVault(
    new Uri(config.GetValue<string>("Keyvault-Importer") ?? throw new ArgumentNullException("Keyvault-Importer is null")),
    new DefaultAzureCredential());

#endregion

builder.Configuration.AddConfiguration(config);
#endregion

#region APPLICATION INSIGHTS
// TODO: Extract this to a package

string? instanceId;

instanceId = Environment.GetEnvironmentVariable("HOSTNAME");
instanceId = instanceId ?? Environment.MachineName;
instanceId = string.IsNullOrEmpty(instanceId) ? Dns.GetHostName() : instanceId;
instanceId = instanceId ?? null;

var resourceAttributes = new Dictionary<string, object> {
        { "service.name", config.GetValue<string>("ApplicationInsights:ApplicationName")
            ?? throw new ArgumentNullException("ApplicationInsights:ApplicationName is null") },
        { "service.instance.id", instanceId
            ?? throw new ArgumentNullException("Could not find instance ID") }
    };

if (isProduction)
{
    builder.Services.AddOpenTelemetry().UseAzureMonitor(
        options =>
        {
            options.ConnectionString = builder.Configuration.GetValue<string>("ApplicationInsightsConnectionString");
        }).ConfigureResource(resourceBuilder =>
        {
            resourceBuilder.AddAttributes(resourceAttributes);
        });

    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddConsole();
    });
}


#endregion

#region DEPENDENCY INJECTION

// Services
builder.Services.AddScoped<IImportService, ImportService>();
builder.Services.AddScoped<IDownloadFileService, DownloadFileService>();
builder.Services.AddScoped<IUpdateVerificationService, UpdateVerificationService>();
builder.Services.AddScoped<IPDFProcessingServices, PDFProcessingServices>();
builder.Services.ImporterCanadaAddData();

builder.Services.AddBlobStorage(
        blobConnectionString: builder.Configuration.GetValue<string>("BlobStorage:URL") ?? throw new ArgumentNullException("BlobStorage:URL Cannot be null"),
        containerName: "canada"
    );

#endregion

#region HTTP FACTORY

builder.Services.ImporterCanadaAddHTTPClients();

#endregion

#region STRONGLY TYPED CONFIGURATIONS

builder.Services.Configure<DownloaderURLs>(
    builder.Configuration.GetSection("DownloaderURLs"));

builder.Services.Configure<TAFLDefinitionTablesOrder>(
    builder.Configuration.GetSection("TAFLDefinitionTables"));

#endregion

#region DB CONTEXTS

var connectionString = builder.Configuration.GetConnectionString("CanadaImporter")
    ?? throw new InvalidOperationException("Connection string 'CanadaImporter' not found.");

builder.Services.AddDbContext<CanadaImporterContext>(options =>
    options.UseSqlServer(
        connectionString,
        x => x.UseNetTopologySuite()
    ));

#endregion

#region AUTOMAPPER
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<TAFLDefinitionProfile>());
#endregion

builder.Build().Run();
