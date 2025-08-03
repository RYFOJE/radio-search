using Microsoft.EntityFrameworkCore;
using Radio_Search.Importer.Canada.Data.Models.History;
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
        public DbSet<ImportHistory> ImportHistories { get; set; }
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
        public DbSet<StationFunctionType> stationFunctionTypes { get; set; }
        public DbSet<StationType> stationTypes { get; set; }
        public DbSet<SubserviceType> SubserviceTypes { get; set; }
        public DbSet<AnalogDigital> AnalogDigital { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Canada_Importer");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AntennaPattern>()
                .Property(e => e.AntennaPatternID)
                .ValueGeneratedNever();

            modelBuilder.Entity<LicenseRecord>(entity =>
            {
                entity.Property(e => e.CanadaLicenseRecordID).ValueGeneratedNever();

                entity.HasKey(e => e.InternalLicenseRecordID)
                    .IsClustered(false);

                entity.Property(e => e.InternalLicenseRecordID)
                    .ValueGeneratedOnAdd();

                entity.HasIndex(u => u.CanadaLicenseRecordID)
                    .IsUnique(false)
                    .IsClustered();

                entity.HasOne(e => e.ImportHistory)
                    .WithMany()
                    .HasForeignKey(e => e.ImportHistoryID)
                    .IsRequired();

                entity.HasIndex(u => u.IsValid);
                entity.HasIndex(u => u.FrequencyMHz);

                entity.Property(e => e.FrequencyMHz).HasPrecision(24, 12);
                entity.Property(e => e.OccupiedBandwidthKHz).HasPrecision(24, 12);
                entity.Property(e => e.TxERPdBW).HasPrecision(24, 12);
                entity.Property(e => e.TxPowerW).HasPrecision(24, 12);
                entity.Property(e => e.TotalLossesDb).HasPrecision(24, 12);
                entity.Property(e => e.AnalogCapacityChannels).HasPrecision(24, 12);
                entity.Property(e => e.DigitalCapacityMbps).HasPrecision(24, 12);
                entity.Property(e => e.AntennaGainDbi).HasPrecision(24, 12);
                entity.Property(e => e.BeamwidthDeg).HasPrecision(24, 12);
                entity.Property(e => e.FrontToBackRatioDb).HasPrecision(24, 12);
                entity.Property(e => e.HeightAboveGroundM).HasPrecision(24, 12);
                entity.Property(e => e.AzimuthMainLobeDeg).HasPrecision(24, 12);
                entity.Property(e => e.VerticalElevationAngleDeg).HasPrecision(24, 12);
                entity.Property(e => e.AntennaStructureHeightM).HasPrecision(24, 12);
                entity.Property(e => e.HorizontalPowerW).HasPrecision(24, 12);
                entity.Property(e => e.VerticalPowerW).HasPrecision(24, 12);

                entity.HasOne(u => u.AntennaPattern)
                    .WithMany()
                    .HasForeignKey(u => u.AntennaPatternID)
                    .IsRequired(false);

                entity.HasOne(u => u.AuthorizationStatus)
                    .WithMany()
                    .HasForeignKey(u => u.AuthorizationStatusID)
                    .IsRequired(false);

                entity.HasOne(u => u.CommunicationType)
                    .WithMany()
                    .HasForeignKey(u => u.CommunicationTypeID)
                    .IsRequired(false);

                entity.HasOne(u => u.ConformityToFrequencyPlan)
                    .WithMany()
                    .HasForeignKey(u => u.ConformityFrequencyPlanID)
                    .IsRequired(false);

                entity.HasOne(u => u.CongestionZone)
                    .WithMany()
                    .HasForeignKey(u => u.CongestionZoneTypeID)
                    .IsRequired(false);

                entity.HasOne(u => u.FiltrationInstalledType)
                    .WithMany()
                    .HasForeignKey(u => u.FiltrationInstalledTypeID)
                    .IsRequired(false);

                entity.HasOne(u => u.ITUClassOfStation)
                    .WithMany()
                    .HasForeignKey(u => u.ITUClassTypeID)
                    .IsRequired(false);

                entity.HasOne(u => u.LicenceType)
                    .WithMany()
                    .HasForeignKey(u => u.LicenseTypeID)
                    .IsRequired(false);

                entity.HasOne(u => u.ModulationType)
                    .WithMany()
                    .HasForeignKey(u => u.ModulationTypeID)
                    .IsRequired(false);

                entity.HasOne(u => u.OperationalStatus)
                    .WithMany()
                    .HasForeignKey(u => u.OperationalStatusID)
                    .IsRequired(false);

                entity.HasOne(u => u.Polarization)
                    .WithMany()
                    .HasForeignKey(u => u.PolarizationTypeID)
                    .IsRequired(false);

                entity.HasOne(u => u.Province)
                    .WithMany()
                    .HasForeignKey(u => u.ProvinceID)
                    .IsRequired(false);

                entity.HasOne(u => u.RegulatoryService)
                    .WithMany()
                    .HasForeignKey(u => u.RegulatoryServiceID)
                    .IsRequired(false);

                entity.HasOne(u => u.Service)
                    .WithMany()
                    .HasForeignKey(u => u.ServiceTypeID)
                    .IsRequired(false);

                entity.HasOne(u => u.StandbyTransmitterInfo)
                    .WithMany()
                    .HasForeignKey(u => u.StandbyTransmitterInformationID)
                    .IsRequired(false);

                entity.HasOne(u => u.StationClass)
                    .WithMany()
                    .HasForeignKey(u => u.StationClassID)
                    .IsRequired(false);

                entity.HasOne(u => u.StationCostCategory)
                    .WithMany()
                    .HasForeignKey(u => u.StationCostCategoryID)
                    .IsRequired(false);

                entity.HasOne(u => u.StationFunction)
                    .WithMany()
                    .HasForeignKey(u => u.StationFunctionID)
                    .IsRequired(false);

                entity.HasOne(u => u.StationType)
                    .WithMany()
                    .HasForeignKey(u => u.StationTypeID)
                    .IsRequired(false);

                entity.HasOne(u => u.Subservice)
                    .WithMany()
                    .HasForeignKey(u => u.SubserviceTypeID)
                    .IsRequired(false);

                entity.HasOne(u => u.AnalogDigital)
                    .WithMany()
                    .HasForeignKey(u => u.AnalogDigitalID)
                    .IsRequired(false);
            });

            modelBuilder.Entity<AntennaPattern>(entity =>
            {
                entity.Property(e => e.AntennaPatternID).ValueGeneratedNever();
            });

            modelBuilder.Entity<AuthorizationStatus>(entity =>
            {
                entity.Property(e => e.AuthorizationStatusID).ValueGeneratedNever();
            });

            modelBuilder.Entity<CommunicationType>(entity =>
            {
                entity.Property(e => e.CommunicationTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<ConformityFrequencyPlan>(entity =>
            {
                entity.Property(e => e.ConformityFrequencyPlanID).ValueGeneratedNever();
            });

            modelBuilder.Entity<CongestionZoneType>(entity =>
            {
                entity.Property(e => e.CongestionZoneTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<FiltrationInstalledType>(entity =>
            {
                entity.Property(e => e.FiltrationInstalledTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<ITUClassType>(entity =>
            {
                entity.Property(e => e.ITUClassTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<LicenseType>(entity =>
            {
                entity.Property(e => e.LicenseTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<ModulationType>(entity =>
            {
                entity.Property(e => e.ModulationTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<OperationalStatus>(entity =>
            {
                entity.Property(e => e.OperationalStatusID).ValueGeneratedNever();
            });

            modelBuilder.Entity<PolarizationType>(entity =>
            {
                entity.Property(e => e.PolarizationTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.Property(e => e.ProvinceID).ValueGeneratedNever();
            });

            modelBuilder.Entity<RegulatoryService>(entity =>
            {
                entity.Property(e => e.RegulatoryServiceID).ValueGeneratedNever();
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.Property(e => e.ServiceTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<StandbyTransmitterInformation>(entity =>
            {
                entity.Property(e => e.StandbyTransmitterInformationID).ValueGeneratedNever();
            });

            modelBuilder.Entity<StationClass>(entity =>
            {
                entity.Property(e => e.StationClassID).ValueGeneratedNever();
            });

            modelBuilder.Entity<StationCostCategory>(entity =>
            {
                entity.Property(e => e.StationCostCategoryID).ValueGeneratedNever();
            });

            modelBuilder.Entity<StationFunctionType>(entity =>
            {
                entity.Property(e => e.StationFunctionTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<StationType>(entity =>
            {
                entity.Property(e => e.StationTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<SubserviceType>(entity =>
            {
                entity.Property(e => e.SubserviceTypeID).ValueGeneratedNever();
            });

            modelBuilder.Entity<AnalogDigital>(entity =>
            {
                entity.Property(e => e.AnalogDigitalID).ValueGeneratedNever();
            });

            modelBuilder.Entity<LicenseRecordHistory>(entity =>
            {
                entity.HasKey(u => u.LicenseRecordHistoryID)
                    .IsClustered(false);

                entity.Property(u => u.LicenseRecordHistoryID)
                    .ValueGeneratedOnAdd();

                entity.HasIndex(u => u.EditedByImportHistoryRecordID)
                    .IsUnique(false);

                entity.HasIndex(u => u.InternalLicenseRecordID)
                    .IsClustered(true);

                entity.HasOne(e => e.LicenseRecord)
                    .WithOne()
                    .HasForeignKey<LicenseRecordHistory>(e => e.InternalLicenseRecordID)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(u => u.EditedByImportHistoryRecord)
                    .WithMany(u => u.AssociatedRecords)
                    .HasForeignKey(u => u.EditedByImportHistoryRecordID)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}

// dotnet ef migrations add "autogenerate" -s .\Radio-Search.Importer.Canada.Function -p .\Radio-Search.Importer.Canada.Data -c CanadaImporterContext -o Migrations
// dotnet ef database update -s .\Radio-Search.Importer.Canada.Function -p .\Radio-Search.Importer.Canada.Data -c CanadaImporterContext