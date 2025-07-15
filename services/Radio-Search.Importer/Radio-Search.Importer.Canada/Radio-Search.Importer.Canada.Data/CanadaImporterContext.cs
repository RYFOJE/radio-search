using Microsoft.EntityFrameworkCore;
using Radio_Search.Importer.Canada.Data.Models.License;

namespace Radio_Search.Importer.Canada.Data
{
    public class CanadaImporterContext : DbContext
    {
        public CanadaImporterContext(DbContextOptions<CanadaImporterContext> options)
            : base(options)
        {
        }

        public DbSet<LicenseRecord> LicenseRecords { get; set; }

        public DbSet<AntennaPattern> AntennaPatterns { get; set; }
        public DbSet<AuthorizationStatus> AuthorizationStatuses { get; set; }
        public DbSet<CommunicationType> CommunicationTypes { get; set; }
        public DbSet<ConformityFrequencyPlan> ConformityFrequencyPlans { get; set; }
        public DbSet<CongestionZoneType> CongestionZoneTypes { get; set; }
        public DbSet<FiltrationInstalledType> FiltrationInstalledTypes { get; set; }
        public DbSet<ITUClassType> ITUClassTypes { get; set; }
        public DbSet<LicenseRecordHistory> LicenseRecordsHistory { get; set; }
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
        public DbSet<StationFunctionType> stationFunctionTypes { get; set; }
        public DbSet<StationType> stationTypes { get; set; }
        public DbSet<SubserviceType> SubserviceTypes { get; set; }
        public DbSet<AnalogDigital> AnalogDigital { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Canada_Importer");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LicenseRecord>(entity =>
            {
                entity.HasKey(u => u.LicenseRecordID);
                //entity.HasIndex(u => u.Location);
                entity.HasIndex(u => u.FrequencyMHz);

                entity.HasOne(u => u.AntennaPattern)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.AuthorizationStatus)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.CommunicationType)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.ConformityToFrequencyPlan)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.CongestionZone)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.FiltrationInstalled)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.ITUClassOfStation)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.LicenceType)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.ModulationType)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.OperationalStatus)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.Polarization)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.Province)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.RegulatoryService)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.Service)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.StandbyTransmitterInfo)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.StationClass)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.StationCostCategory)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.StationFunction)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.StationType)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.Subservice)
                    .WithMany()
                    .IsRequired(false);

                entity.HasOne(u => u.AnalogDigital)
                    .WithMany()
                    .IsRequired(false);
            });

            modelBuilder.Entity<LicenseRecordHistory>(entity =>
            {
                entity.HasOne(u => u.LicenseRecord)
                    .WithMany(u => u.History)
                    .IsRequired();

                entity.HasOne(u => u.ImportHistoryRecord)
                    .WithMany(u => u.AssociatedRecords)
                    .IsRequired();
            });
        }
    }
}

// dotnet ef migrations add "initial_commit" -s .\Radio-Search.Importer.Canada.Function -p .\Radio-Search.Importer.Canada.Data -c CanadaImporterContext -o Migrations
// dotnet ef database update -s .\Radio-Search.Importer.Canada.Function -p .\Radio-Search.Importer.Canada.Data -c CanadaImporterContext