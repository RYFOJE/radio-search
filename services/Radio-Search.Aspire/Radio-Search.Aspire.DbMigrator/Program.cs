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

        var connString = builder.Configuration.GetConnectionString("importer-canada")
            ?? throw new InvalidOperationException("Connection string 'importer-canada-db' not found.");

        builder.Services.AddDbContext<CanadaImporterContext>(options =>
            options.UseNpgsql(connString, sql =>
            {
                sql.UseNetTopologySuite();
                sql.MigrationsHistoryTable("__EFMigrationsHistory", "canada_importer");
            })
        );

        builder.Services.AddHostedService<ImporterCanada>();

        var host = builder.Build();
        host.Run();
    }
}
