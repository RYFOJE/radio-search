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
        public const string DATABASE_NAME = "importer_canada";


        public CanadaImporterContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production";

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

            var dbConnString = string.Format(
                configuration.GetValue<string>("PostgresqlConnectionTemplate") ?? throw new InvalidOperationException("PostgresqlConnectionTemplate is null"),
                configuration.GetValue<string>("PostgresqlDbUrl") ?? throw new InvalidOperationException("PostgresqlDbUrl is null"),
                DATABASE_NAME,
                configuration.GetValue<string>("PostgresqlAdminUsername") ?? throw new InvalidOperationException("PostgresqlAdminUsername is null"),
                configuration.GetValue<string>("PostgresqlAdminPassword") ?? throw new InvalidOperationException("PostgresqlAdminPassword is null")
            );

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