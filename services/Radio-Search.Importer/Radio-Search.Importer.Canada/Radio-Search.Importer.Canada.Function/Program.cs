using Azure.Identity;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Radio_Search.Importer.Canada.Data;
using Radio_Search.Importer.Canada.Services;
using Radio_Search.Importer.Canada.Services.Configuration;
using Radio_Search.Importer.Canada.Services.Data;
using Radio_Search.Importer.Canada.Services.Implementations;
using Radio_Search.Importer.Canada.Services.Implementations.TAFLDefinitionImport;
using Radio_Search.Importer.Canada.Services.Implementations.TAFLImport;
using Radio_Search.Importer.Canada.Services.Interfaces;
using Radio_Search.Importer.Canada.Services.Interfaces.TAFLDefinitionImport;
using Radio_Search.Importer.Canada.Services.Interfaces.TAFLImport;
using Radio_Search.Importer.Canada.Services.Mappings;
using Radio_Search.Importer.Canada.Services.Validators;
using Radio_Search.Utils.BlobStorage;
using Radio_Search.Utils.MessageBroker.ConfigurationSetupExtensions;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Create a logger for startup
using var loggerFactory = LoggerFactory.Create(logging =>
{
    logging.AddConsole();
});
var logger = loggerFactory.CreateLogger("Startup");

logger.LogInformation("LOADING. PLEASE AT LEAST SHOW UP");

#region VALUES

var options = new DefaultAzureCredentialOptions { Diagnostics = { IsLoggingContentEnabled = true } };
var credential = new DefaultAzureCredential(options);

logger.LogInformation("FETCHED AZURE CREDENTIALS");

#region BOOTSTRAPPING

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production";
var isProduction = string.Equals(environment, "production");

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

logger.LogInformation("Loaded appsettings");

#endregion

#region APP CONFIG

// Common
try
{
    var uri = config.GetValue<string>("AppConfig:Common:URL") ?? throw new ArgumentNullException("AppConfig:Common:URL is null");
    var sentinelLabel = config.GetValue<string>("AppConfig:Common:Sentinel") ?? throw new ArgumentNullException("AppConfig:Common:Sentinel is null");
    var refreshInterval = config.GetValue("AppConfig:Common:RefreshInterval", 5);

    logger.LogInformation("Initializing Common AppConfig");
    builder.Configuration.AddAzureAppConfiguration(options =>
    {


        options.Connect(new Uri(uri), new DefaultAzureCredential());

        options.Select(KeyFilter.Any, LabelFilter.Null);

        options.ConfigureRefresh(refreshOptions =>
        {
            refreshOptions.Register(sentinelLabel, refreshAll: true);
            refreshOptions.SetRefreshInterval(TimeSpan.FromMinutes(refreshInterval));
        });
    });
    logger.LogInformation("Loaded common appconfig");
}
catch (Exception ex)
{
    logger.LogError(ex, "Failed to initialize Common AppConfig");
    throw;
}

// Importers
try
{
    var uri = config.GetValue<string>("AppConfig:Importer:URL") ?? throw new ArgumentNullException("AppConfig:Importer:URL is null");
    var sentinelLabel = config.GetValue<string>("AppConfig:Importer:Sentinel") ?? throw new ArgumentNullException("AppConfig:Importer:Sentinel is null");
    var keyPrefix = config.GetValue<string>("AppConfig:Importer:Prefix") ?? throw new ArgumentNullException("AppConfig:Importer:Prefix is null");
    var refreshInterval = config.GetValue("AppConfig:Importer:RefreshInterval", 5);

    logger.LogInformation("Initializing Importer AppConfig");
    builder.Configuration.AddAzureAppConfiguration(options =>
    {


        options.Connect(new Uri(uri), new DefaultAzureCredential());

        options.Select($"{keyPrefix}:*", LabelFilter.Null);
        options.TrimKeyPrefix($"{keyPrefix}:");

        options.ConfigureRefresh(refreshOptions =>
        {
            refreshOptions.Register(sentinelLabel, refreshAll: true);
            refreshOptions.SetRefreshInterval(TimeSpan.FromMinutes(refreshInterval));
        });
    });
    logger.LogInformation("Loaded importer appconfig");
}
catch (Exception ex)
{
    logger.LogError(ex, "Failed to initialize Importer AppConfig");
    throw;
}

#endregion

#region KEYVAULT

builder.Configuration.AddAzureKeyVault(
    new Uri(config.GetValue<string>("Keyvault-Importer") ?? throw new ArgumentNullException("Keyvault-Importer is null")),
    new DefaultAzureCredential());

logger.LogInformation("Loaded keyvault");

#endregion

builder.Configuration.AddConfiguration(config);

#endregion

#region DEPENDENCY INJECTION

// Services
builder.Services.AddScoped<IImportManagerService, ImportManagerService>();
builder.Services.AddScoped<IDownloadFileService, DownloadFileService>();
builder.Services.AddScoped<IPDFProcessingServices, PdfProcessingServices>();
builder.Services.AddScoped<ITAFLDefinitionImportService, TaflDefinitionImportService>();
builder.Services.AddScoped<IPreprocessingService, PreprocessingService>();
builder.Services.AddScoped<IProcessingService, ProcessingService>();
builder.Services.ImporterCanadaAddData();
builder.Services.AddScoped<IValidator<TaflEntryRawRow>, TAFLEntryRawRowValidator>();

builder.Services.AddAzureServiceBusClient(new() { 
    ServiceBusUrl = config.GetConnectionString("CanadaImporterServiceBus") ?? throw new ArgumentNullException()
});
builder.Services.AddAzureWriterFactory();

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

builder.Services.Configure<FileLocations>(
    builder.Configuration.GetSection("FileLocations"));

builder.Services.Configure<ServiceBusDefinitions>(
    builder.Configuration.GetSection("ServiceBusDefinitions"));

#endregion

#region DB CONTEXTS

var dbConnString = String.Format(
    builder.Configuration.GetValue<string>("PostgresqlConnectionTemplate")
        ?? throw new InvalidOperationException("PostgresqlConnectionTemplate is null"),
    builder.Configuration.GetValue<string>("CanadaDbUrl")
        ?? throw new InvalidOperationException("CanadaDbUrl is null"),
    builder.Configuration.GetValue<string>("CanadaDbName")
        ?? throw new InvalidOperationException("CanadaDbName is null"),
    builder.Configuration.GetValue<string>("CanadaDbUsername")
        ?? throw new InvalidOperationException("CanadaDbUsername is null"),
    builder.Configuration.GetValue<string>("CanadaDbPassword")
        ?? throw new InvalidOperationException("CanadaDbPassword is null")
);

builder.Services.AddDbContext<CanadaImporterContext>(options =>
    options.UseNpgsql(
        dbConnString,
        sqlOptions =>
        {
            sqlOptions.UseNetTopologySuite();
            sqlOptions.MigrationsHistoryTable(
                tableName: "__EFMigrationsHistory",
                schema: "canada_importer"
            );
            sqlOptions.CommandTimeout(180);
        }
    )
    .EnableSensitiveDataLogging(false)
);

#endregion

#region AUTOMAPPER
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<TAFLDefinitionProfile>());
builder.Services.AddAutoMapper(cfg =>cfg.AddProfile<TAFLRowProfile>());
#endregion

//builder.Services.AddHostedService<RunAtStart>();

await builder.Build().RunAsync();
