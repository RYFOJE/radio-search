using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;

namespace Radio_Search.Importer.Canada.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CanadaImporterContext>
    {

        /// I KNOW THESE SHOULD BE IN A CONFIG FILE BUT I AM SICK OF FIGHTING WITH THE THINGS TO GET THIS TO WORK MY INNER DEVIL IS TAKING OVER
        /// RAAAAGH
        public const string KV_URL = "https://rds-administration.vault.azure.net/";
        public const string APP_CONFIG_URL = "https://rds-config.azconfig.io";

        public CanadaImporterContext CreateDbContext(string[] args)
        {
            // Build full configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddAzureKeyVault(
                    new Uri(KV_URL),
                    new DefaultAzureCredential())
                .AddAzureAppConfiguration(options =>
                {
                    var uri = APP_CONFIG_URL;

                    options.Connect(new Uri(uri), new DefaultAzureCredential())
                           .Select(KeyFilter.Any, LabelFilter.Null);
                })
                .Build();

            var dbConnString = configuration.GetConnectionString("importer-canada")
                ?? throw new InvalidOperationException("ConnectionStrings:importer-canada is null");

            var optionsBuilder = new DbContextOptionsBuilder<CanadaImporterContext>();
            optionsBuilder.UseNpgsql(
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
            .EnableSensitiveDataLogging(false);

            return new CanadaImporterContext(optionsBuilder.Options);
        }
    }
}