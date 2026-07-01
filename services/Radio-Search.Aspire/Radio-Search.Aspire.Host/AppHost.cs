using Aspire.Hosting.Azure;
using Azure.Provisioning.PostgreSql;
using Azure.Provisioning.ServiceBus;
using Radio_Search.Aspire.Host.Helpers;

var builder = DistributedApplication.CreateBuilder(args);


#region Database

var postgres = builder.AddAzurePostgresFlexibleServer("rds-spatial-db")
    .ConfigureInfrastructure(infra =>
    {
        var flexibleServer = infra.GetProvisionableResources()
                                .OfType<PostgreSqlFlexibleServer>()
                                .Single();

        flexibleServer.Sku = new PostgreSqlFlexibleServerSku
        {
            Name = "Standard_B1ms",                                 // compute size
            Tier = PostgreSqlFlexibleServerSkuTier.Burstable
        };
        flexibleServer.HighAvailability = new PostgreSqlFlexibleServerHighAvailability
        {
            Mode = PostgreSqlFlexibleServerHighAvailabilityMode.Disabled
        };
        flexibleServer.Storage = new PostgreSqlFlexibleServerStorage
        {
            StorageSizeInGB = 32,
            AutoGrow = StorageAutoGrow.Disabled,
            StorageType = PostgreSqlFlexibleServersStorageType.PremiumLRS,
            Tier = PostgreSqlManagedDiskPerformanceTier.P4          // P4 (120 iops)
        };
        flexibleServer.Backup = new PostgreSqlFlexibleServerBackupProperties
        {
            BackupRetentionDays = 7,
            GeoRedundantBackup = PostgreSqlFlexibleServerGeoRedundantBackupEnum.Disabled
        };
    })
    .RunAsContainer(container =>
    {
        container.WithImage("postgis/postgis");
        container.WithLifetime(ContainerLifetime.Persistent);
        container.WithDataVolume();
        container.WithPgAdmin();
    });

var importerCanadaDb = postgres.AddDatabase("importer-canada");

var migrator = builder.AddProject<Projects.Radio_Search_Aspire_DbMigrator>("db-migrator")
    .WithReference(importerCanadaDb)
    .WaitFor(postgres);

#endregion

#region Servicebus

var importerServiceBus = builder.AddAzureServiceBus("importer-servicebus")
    .ConfigureInfrastructure(infra =>
    {
        var serviceBusNamespace = infra.GetProvisionableResources()
                                        .OfType<ServiceBusNamespace>()
                                        .Single();

        serviceBusNamespace.Sku = new ServiceBusSku
        {
            Name = ServiceBusSkuName.Standard
        };
        serviceBusNamespace.Name = "rds-importers";
    })
    .RunAsEmulator();

// The topic name must be "canada" to match the ServiceBusTrigger attributes in the Function app.
var sbImporterCanadaTopic = importerServiceBus.AddServiceBusTopic("canada");

// Each subscription only receives messages whose "Target" application property matches its own
// name (see AzureServiceBusWriter.cs), so publishing one message type doesn't fan out to every
// Function trigger on the topic.
AddFilteredSubscription(sbImporterCanadaTopic, "ChunkProcessingComplete");
AddFilteredSubscription(sbImporterCanadaTopic, "ChunkReady");
AddFilteredSubscription(sbImporterCanadaTopic, "DownloadComplete");
AddFilteredSubscription(sbImporterCanadaTopic, "ImportStart");

static void AddFilteredSubscription(IResourceBuilder<AzureServiceBusTopicResource> topic, string subscriptionName)
{
    topic.AddServiceBusSubscription(subscriptionName)
        .WithProperties(subscription =>
        {
            subscription.Rules.Add(new AzureServiceBusRule(subscriptionName)
            {
                FilterType = AzureServiceBusFilterType.CorrelationFilter,
                CorrelationFilter = new AzureServiceBusCorrelationFilter
                {
                    Properties = { ["Target"] = subscriptionName }
                }
            });
        });
}

importerServiceBus.WithServiceBusDashboardCommands("canada",
    "ImportStart", "ChunkReady", "DownloadComplete", "ChunkProcessingComplete");

#endregion

#region Storage

var functionStorage = builder.AddAzureStorage("functionStorage")
    .RunAsEmulator();

var importStorage = builder.AddAzureStorage("importerStorage")
    .RunAsEmulator(azurite =>
    {
        azurite.WithLifetime(ContainerLifetime.Persistent);
        azurite.WithDataVolume();
    })
    .AddBlobContainer("canada");

#endregion


// WithReference() only feeds Aspire client integrations, not Functions triggers/bindings, so the
// trigger's `Connection = "sb_importer"` setting is wired explicitly via WithEnvironment().
// Locally (emulator) this resolves to a full connection string on "sb_importer".
// When published to Azure this resolves to the namespace URI on "sb_importer__fullyQualifiedNamespace"
// for identity-based (DefaultAzureCredential) auth.
var sbImporterConnectionEnvName = "sb_importer" + (builder.ExecutionContext.IsPublishMode ? "__fullyQualifiedNamespace" : "");

builder.AddAzureFunctionsProject<Projects.Radio_Search_Importer_Canada_Function>("importer-canada-function")
    .WithHostStorage(functionStorage)
    .WaitFor(postgres)
    .WaitForCompletion(migrator)
    .WithReference(importerCanadaDb)
    .WithReference(sbImporterCanadaTopic)
    .WaitFor(sbImporterCanadaTopic)
    .WithEnvironment(sbImporterConnectionEnvName, importerServiceBus.Resource.ConnectionStringExpression)
    .WithReference(importStorage)
    .WaitFor(importStorage)
    .WithRoleAssignments(importStorage, AzureStorageRole.StorageBlobDataReader);

builder.Build().Run();
