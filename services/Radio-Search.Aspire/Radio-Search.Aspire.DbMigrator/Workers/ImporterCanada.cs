using Microsoft.EntityFrameworkCore;
using Radio_Search.Importer.Canada.Data;

namespace Radio_Search.Aspire.DbMigrator.Workers
{
    public class ImporterCanada(
        IServiceProvider sp,
        IHostApplicationLifetime lifetime) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CanadaImporterContext>();
            await db.Database.ExecuteSqlRawAsync("CREATE SCHEMA IF NOT EXISTS canada_importer", ct);
            await db.Database.MigrateAsync(ct);
            lifetime.StopApplication();   // done, shut down
        }
    }
}
