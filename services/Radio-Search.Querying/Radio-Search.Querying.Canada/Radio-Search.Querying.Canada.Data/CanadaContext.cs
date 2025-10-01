using Microsoft.EntityFrameworkCore;
using Radio_Search.Canada.Models.History;
using Radio_Search.Canada.Models.ImportInfo;
using Radio_Search.Canada.Models.License;

namespace Radio_Search.Querying.Canada.Data
{
    public class CanadaContext : DbContext
    {
        public CanadaContext(DbContextOptions<CanadaContext> options)
            : base(options)
        {
        }

        public DbSet<LicenseRecord> LicenseRecords { get; set; }
        public DbSet<ImportJob> ImportJobs { get; set; }
        public DbSet<ImportJobChunkFile> ImportJobChunkFiles { get; set; }
        public DbSet<ImportJobStats> ImportJobStats { get; set; }
        public DbSet<LicenseRecordHistory> LicenseRecordsHistory { get; set; }

        public DbSet<AntennaPattern> AntennaPatterns { get; set; }
        public DbSet<AuthorizationStatus> AuthorizationStatuses { get; set; }
        public DbSet<CommunicationType> CommunicationTypes { get; set; }
        public DbSet<ConformityFrequencyPlan> ConformityFrequencyPlans { get; set; }
        public DbSet<CongestionZoneType> CongestionZoneTypes { get; set; }
        public DbSet<FiltrationInstalledType> FiltrationInstalledTypes { get; set; }
        public DbSet<ITUClassType> ITUClassTypes { get; set; }
        public DbSet<LicenseType> LicenseTypes { get; set; }
        public DbSet<ModulationType> ModulationTypes { get; set; }
        public DbSet<OperationalStatus> OperationStatuses { get; set; }
        public DbSet<PolarizationType> PolarizationTypes { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<RegulatoryService> RegulatoryServices { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<StandbyTransmitterInformation> StandbyTransmitterInformation { get; set; }
        public DbSet<StationClass> StationClasses { get; set; }
        public DbSet<StationCostCategory> StationCostCategories { get; set; }
        public DbSet<StationFunctionType> StationFunctionTypes { get; set; }
        public DbSet<StationType> StationTypes { get; set; }
        public DbSet<SubserviceType> SubserviceTypes { get; set; }
        public DbSet<AnalogDigital> AnalogDigital { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("canada_importer");

            base.OnModelCreating(modelBuilder);
        }

        // Prevent accidental writes
        public override int SaveChanges() => throw new NotSupportedException("Readonly context");
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            throw new NotSupportedException("Readonly context");
    }
}

