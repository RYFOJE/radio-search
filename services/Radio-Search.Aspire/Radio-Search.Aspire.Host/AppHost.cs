using Azure.Provisioning.PostgreSql;

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
        container
            .WithImage("postgis/postgis")
            .WithDataVolume();

        container.WithPgAdmin();
    });

var importerCanadaDb = postgres.AddDatabase("importer-canada");

var migrator = builder.AddProject<Projects.Radio_Search_Aspire_DbMigrator>("db-migrator")
    .WithReference(importerCanadaDb)
    .WaitFor(postgres);

#endregion

#region Servicebus



#endregion

var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();

builder.AddAzureFunctionsProject<Projects.Radio_Search_Importer_Canada_Function>("importer-canada-function")
    .WithHostStorage(storage)
    .WaitFor(postgres)
    .WaitFor(migrator)
    .WithReference(importerCanadaDb);

builder.Build().Run();
