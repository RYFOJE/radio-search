using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite;
using Radio_Search.Aspire.DbMigrator.Workers;
using Radio_Search.Importer.Canada.Data;

namespace Radio_Search.Aspire.DbMigrator;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.AddServiceDefaults();

        builder.AddNpgsqlDbContext<CanadaImporterContext>(
            "importer-canada",
            configureDbContextOptions: options =>
            {
                options.UseNpgsql(npgsql =>
                {
                    npgsql.UseNetTopologySuite();
                    npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "canada_importer");
                });
            }
        );

        builder.Services.AddHostedService<ImporterCanada>();

        var host = builder.Build();
        host.Run();
    }
}
